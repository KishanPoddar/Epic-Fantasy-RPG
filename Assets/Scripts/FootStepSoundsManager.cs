using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class FootStepSoundsManager : MonoBehaviour
{
    [SerializeField] private Transform rayCastObj;
    [SerializeField]
    private LayerMask FloorLayer;
    [SerializeField]
    private TextureSound[] TextureSounds;
    private CharacterController Controller;
    private AudioSource audioSource;
    private ThirdPersonController control;
    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        control = GetComponent<ThirdPersonController>();

    }

    private void Start()
    {
        //StartCoroutine(CheckGround());
    }

    public IEnumerator CheckGround()
    {
        
            if(Controller.velocity == Vector3.zero)
                audioSource.Stop();
            else if (control.Grounded && Controller.velocity != Vector3.zero &&
                Physics.Raycast(rayCastObj.position,
                    Vector3.down,
                    out RaycastHit hit,
                    1f,
                    FloorLayer)
                )
            {
                if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                {
                    yield return StartCoroutine(PlayFootstepSoundFromTerrain(terrain, hit.point));
                }
                else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    yield return StartCoroutine(PlayFootstepSoundFromRenderer(renderer));
                }
            }

            yield return null;
        
    }

    private IEnumerator PlayFootstepSoundFromTerrain(Terrain Terrain, Vector3 HitPoint)
    {
        Vector3 terrainPosition = HitPoint - Terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(
            terrainPosition.x / Terrain.terrainData.size.x,
            0,
            terrainPosition.z / Terrain.terrainData.size.z
        );

        int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

        float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        int primaryIndex = 0;
        for (int i = 1; i < alphaMap.Length; i++)
        {
            if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
            {
                primaryIndex = i;
            }
        }

        foreach (TextureSound textureSound in TextureSounds)
        {
            if (textureSound.Albedo == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
            {
                AudioClip clip = GetClipFromTextureSound(textureSound);
                AudioSource.PlayClipAtPoint(clip, rayCastObj.transform.position, control.FootstepAudioVolume);
                if(Controller.velocity != Vector3.zero)
                {
                    yield return new WaitForSeconds(clip.length);
                }
                else
                {
                    audioSource.Stop();
                    break;
                }
            }
        }
       
    }

    private IEnumerator PlayFootstepSoundFromRenderer(Renderer Renderer)
    {
        foreach (TextureSound textureSound in TextureSounds)
        {
            if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
            {
                AudioClip clip = GetClipFromTextureSound(textureSound);

                AudioSource.PlayClipAtPoint(clip, rayCastObj.transform.position, control.FootstepAudioVolume);
                yield return new WaitForSeconds(clip.length);
                break;
            }
        }
    }

    private AudioClip GetClipFromTextureSound(TextureSound TextureSound)
    {
        int clipIndex = Random.Range(0, TextureSound.Clips.Length);
        return TextureSound.Clips[clipIndex];
    }

    [System.Serializable]
    private class TextureSound
    {
        public Texture Albedo;
        public AudioClip[] Clips;
    }
}