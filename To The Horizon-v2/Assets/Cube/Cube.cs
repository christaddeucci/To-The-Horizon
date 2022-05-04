using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Cube : MonoBehaviour
{

    FMODUnity.StudioEventEmitter Beat2; //used to manipulate fmod event

   private FMOD.Studio.EventInstance frequency;
   private FMOD.DSP mFFT;
   const int WindowSize = 1024;
   public float lerp;
   public float lerp2;
   public float length;

    public Material skybox_material1; 
    public Material skybox_material2;
    public Material sun_material;

    public Material sun_material2;
    public  Color topColor;
    public Color botColor;

    public Color sunFresnelColor;
    public Color cloudColor;

    public float distortionSpeed;
    public float distortionScale;

    public float sunDensity;


    //area color: 0, 1, 2, damage taken: 3
    float[,] topArr = new float[,] {{0.9f, 0.29f, 1f}, {0.29f, 0.29f, 1f}, {0.5f, 0.29f, 1f}, {1f, 0f, 1f}}; //top skybox colors
    float[,] botArr = new float[,] {{0.12f, 0.29f, 1f}, {0.5f, 0.29f, 1f}, {0.8f, 0.29f, 1f}, {0f, 0f, 1f}}; //bottom skybox colors

    float[,] fresnelArr = new float[,] {{0.2f, 0.9f, 1f}, {0.1f, 0.9f, 1f}, {0.8f, 0.29f, 1f}, {0f, 0f, 1f}}; //sun fresnel colors
    float[,] sunArr = new float[,] {{0.2f, 0.29f, 1f}, {0.6f, 0.9f, 1f}, {0.5f, 0.29f, 1f}, {1f, 0f, 1f}}; //sun base color

    float[,] cloudArr = new float[,] {{0.3f, 0.4f, 0.2f}, {0.9f, 0.9f, 0.5f}, {0.9f, 0.2f, 0.3f}, {1f, 0f, 0.5f}}; //cloud Colors

    int score = 0;


    void Awake(){
        Beat2 = GetComponent<FMODUnity.StudioEventEmitter>();
        
        
    }

    void Start(){

  
        lerp = Mathf.PingPong(Time.deltaTime, 1.0f) / 100.0f;
        lerp2 = Mathf.PingPong(Time.deltaTime, 1.0f) / 2.0f;


        //start beat
        Beat2.Play();
        Beat2.SetParameter("Damage", 1, false);
        Beat2.SetParameter("Blink", 1, false);


        //skybox
  
        skybox_material1.SetColor("_BotColor", Color.HSVToRGB(botArr[0, 0], botArr[0, 1], botArr[0, 2]));
        sun_material.SetColor("_SunFresnelColor", Color.HSVToRGB(fresnelArr[1, 0], fresnelArr[1, 1], fresnelArr[1, 2]));
        sun_material.SetColor("_SunBaseColor", Color.HSVToRGB(sunArr[1, 0], sunArr[1, 1], sunArr[1, 2]));
        RenderSettings.skybox = skybox_material1;
       


        //get beat frequency
        //https://fmod.com/resources/documentation-unity?version=2.02&page=examples-spectrum-analysis.html
        if(FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out mFFT) == FMOD.RESULT.OK){
            //mFFT.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.HANNING);
            //mFFT.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, WindowSize * 2);
            FMODUnity.RuntimeManager.StudioSystem.flushCommands(); //garbage collection
            FMOD.Studio.Bus selectedBus = FMODUnity.RuntimeManager.GetBus("bus:/"); //get bus for master channel of beat2
            if(selectedBus.hasHandle()){

                FMOD.ChannelGroup channelGroup;
                if(selectedBus.getChannelGroup(out channelGroup) == FMOD.RESULT.OK){
                   channelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, mFFT); //add channel
                }
            }
        }
        


    

    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("score: " + score);
        //Beat2.SetParameter("Damage", 1, false); //reset damage parameter effect
        //Beat2.SetParameter("Blink", 1, false);
        /*if(Input.GetKey("w")){
            //progress to next segment
            Beat2.SetParameter("Damage", 0, false);
            score = 0; //reset track when hit
            Beat2.SetParameter("Score", 0, false);

            skybox_material1.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
            skybox_material1.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
            skybox_material2.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[3, 0], cloudArr[3, 1], cloudArr[3, 2]));
            skybox_material2.SetColor("_SunFresnelColor", Color.HSVToRGB(fresnelArr[3, 0], fresnelArr[3, 1], fresnelArr[3, 2]));
            skybox_material2.SetColor("_TopColor", Color.HSVToRGB(topArr[0, 0], topArr[0, 1], topArr[0, 2]));
            skybox_material2.SetColor("_BotColor", Color.HSVToRGB(botArr[0, 0], botArr[0, 1], botArr[0, 2]));

            
            //sun_material2.SetColor("_SunFresnelColor", Color.HSVToRGB(fresnelArr[3, 0], fresnelArr[3, 1], fresnelArr[3, 2]));
            //sun_material2.SetColor("_SunBaseColor", Color.HSVToRGB(sunArr[3, 0], sunArr[3, 1], sunArr[3, 2]));


            
            RenderSettings.skybox.Lerp(skybox_material1, skybox_material2, lerp2);

        }*/
        /*if(Input.GetKey("s")){
            Beat2.SetParameter("Blink", 0, false);
        }
        score += 1;
        if(score >= 10000 && score < 16000){
                skybox_material1.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                skybox_material1.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                //sun_material.SetColor("_SunFresnelColor", Color.HSVToRGB(fresnelArr[1, 0], fresnelArr[1, 1], fresnelArr[1, 2]));
                //rand = Random.Range(0,2);
                skybox_material2.SetColor("_TopColor", Color.HSVToRGB(topArr[1, 0], topArr[1, 1], topArr[1, 2]));
                skybox_material2.SetColor("_BotColor", Color.HSVToRGB(botArr[1, 0], botArr[1, 1], botArr[1, 2]));
                RenderSettings.skybox.Lerp(skybox_material1, skybox_material2, lerp);
           
            Beat2.SetParameter("Score", 25, false);
           
        }*/

        if (score >= 16000 && score <= 19000){
            Beat2.SetParameter("Score", 51, false);

                skybox_material1.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                skybox_material1.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                //sun_material.SetColor("_SunFresnelColor", Color.HSVToRGB(fresnelArr[2, 0], fresnelArr[2, 1], fresnelArr[2, 2]));
                //rand = Random.Range(0,2);
                skybox_material2.SetColor("_TopColor", Color.HSVToRGB(topArr[2, 0], topArr[2, 1], topArr[2, 2]));
                skybox_material2.SetColor("_BotColor", Color.HSVToRGB(botArr[2, 0], botArr[2, 1], botArr[2, 2]));
                RenderSettings.skybox.Lerp(skybox_material1, skybox_material2, lerp);
             
        }

       

        //updating frequency 
        if(mFFT.hasHandle()){
            if (mFFT.getParameterFloat((int)FMOD.DSP_FFT.DOMINANT_FREQ, out length) == FMOD.RESULT.OK)
            {
                //FMOD.DSP_PARAMETER_FFT fftData = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(FMOD.DSP_PARAMETER_FFT));
                //Debug.Log("dominant freq:" + length);
                //change shader attributes based on dominant frequency 
                length = length/2;
                if(length < 500){
                    skybox_material2.SetFloat("_DistortionSpeed", -0.06f);
                    skybox_material2.SetFloat("_DistortionScale", 10f);
                    //sun_material.SetFloat("_SunDensity", lerp2);
                    skybox_material2.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[2, 0], cloudArr[2, 1], cloudArr[2, 2]));
                    
                    skybox_material1.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                    skybox_material1.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                    skybox_material2.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                    skybox_material2.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                    RenderSettings.skybox.Lerp(skybox_material1, skybox_material2, lerp);
                    //RenderSettings.skybox.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[2, 0], cloudArr[2, 1], cloudArr[2, 2]));
               
                }
                if(length >= 500 && length < 3000){
               
                    skybox_material2.SetFloat("_DistortionSpeed", -0.046f);
                    skybox_material2.SetFloat("_DistortionScale", 8f);
                    //sun_material.SetFloat("_SunDensity", lerp2);
                    skybox_material2.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[1, 0], cloudArr[1, 1], cloudArr[1, 2]));
                    
                    skybox_material1.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                    skybox_material1.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                    skybox_material2.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                    skybox_material2.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                    RenderSettings.skybox.Lerp(skybox_material1, skybox_material2, lerp);
                    //RenderSettings.skybox.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[1, 0], cloudArr[1, 1], cloudArr[1, 2]));
                    
                }
                if(length >= 5000){
                    skybox_material2.SetFloat("_DistortionSpeed", -0.04f);
                    skybox_material2.SetFloat("_DistortionScale", 5f);
                    //sun_material.SetFloat("_SunDensity", lerp2);
                    skybox_material2.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[0, 0], cloudArr[0, 1], cloudArr[0, 2]));
                    
                    skybox_material1.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                    skybox_material1.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                    skybox_material2.SetColor("_TopColor",RenderSettings.skybox.GetColor("_TopColor"));
                    skybox_material2.SetColor("_BotColor",RenderSettings.skybox.GetColor("_BotColor"));
                    RenderSettings.skybox.Lerp(skybox_material1, skybox_material2, lerp);
                    //RenderSettings.skybox.SetColor("_CloudColor", Color.HSVToRGB(cloudArr[1, 0], cloudArr[1, 1], cloudArr[1, 2]));
                    
                    
                }
            }    
        }
        
    }
}
