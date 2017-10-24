/// <summary>
/// Sound manager.
/// This script use for manager all sound(bgm,sfx) in game
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	[System.Serializable]
	public class SoundVolume{
		public float BGM = 0.5f;
		public float Voice = 0.5f;
		public float SE = 0.5f;
		public bool Mute = false;

		public void Init(){
			BGM = 0.5f;
			Voice = 0.5f;
			SE = 0.5f;
			Mute = false;
		}
	}

    [System.Serializable]
    public class SoundGroup
    {
        public AudioClip audioClip;
        public string soundName;
    }

    //public AudioSource bgmSound;
    //public AudioSource seSound;
	//public AudioListener alistener;

    // 音量
    public SoundVolume volume = new SoundVolume();
	// === AudioSource ===
	// BGM
	private AudioSource BGMsource;
	// SE
	private AudioSource[] SEsources;
	// 音声
	//public AudioSource[] VoiceSources;

	// === AudioClip ===
	// BGM
	public List<SoundGroup> BGM = new List<SoundGroup>();
    // SE
    public List<SoundGroup> SE = new List<SoundGroup>();
    // 音声
    //public AudioClip[] Voice;

	
	//public List<SoundGroup> sound_List = new List<SoundGroup>();
	
	private static SoundManager _instance;
	private float ptime = 0.0f;

	public static SoundManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = (SoundManager) FindObjectOfType(typeof(SoundManager));

				if (_instance == null)
				{
					Debug.LogError("SoundManager Instance Error");
				}
			}

			return _instance;
		}
	}
	
	void Awake (){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
		if( obj.Length > 1 ){
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
		}

		// BGM AudioSource
		BGMsource = gameObject.AddComponent<AudioSource>();
		BGMsource.loop = true;

		// SE AudioSource
		SEsources = new AudioSource[SE.Count];
		for(int i = 0 ; i < SEsources.Length ; i++ ){
			SEsources[i] = gameObject.AddComponent<AudioSource>();
		}

		// 音声 AudioSource
		//VoiceSources = new AudioSource[Voice.Length];
		//for(int i = 0 ; i < VoiceSources.Length ; i++ ){
		//	VoiceSources[i] = gameObject.AddComponent<AudioSource>();
		//}
	}

	void Start(){
		//StartCoroutine (StartBGM());

	}
    /*
	IEnumerator StartBGM()
	{
		yield return new WaitForSeconds(0.5f);

		while(!PatternSystem.instance || PatternSystem.instance.loadingComplete == false)
		{
			yield return 0;
		}
		Debug.Log("PlayBGM");
		PlayBGM(0);
	}
    */

	void Update () {
		BGMsource.mute = volume.Mute;
		foreach(AudioSource source in SEsources ){
			source.mute = volume.Mute;
		}
		//foreach(AudioSource source in VoiceSources ){
		//	source.mute = volume.Mute;
		//}

		// ボリューム設定
		BGMsource.volume = volume.BGM;
		foreach(AudioSource source in SEsources ){
			source.volume = volume.SE;
		}
		//foreach(AudioSource source in VoiceSources ){
		//	source.volume = volume.Voice;
		//}
	}

	// ***** BGM再生 *****
	// BGM再生
	public void PlayBGM(string name, bool loop)
    {
		//AudioListener.pause = true;
		AudioListener.pause = false;
        //Debug.Log("PlayBGM: name = " + name);
        SoundGroup bgm = null;
        foreach (SoundGroup sound in BGM)
        {
            if (sound.soundName.Equals(name)) {
                bgm = sound;
                break;
            }
        }
        if (bgm == null)
        {
			return;
		}
		// 同じBGMの場合は何もしない
		if( BGMsource.clip == bgm.audioClip)
        {
			BGMsource.time = 0;
			BGMsource.Play();
			return;
		}
		BGMsource.Stop();
		BGMsource.clip = bgm.audioClip;
        BGMsource.loop = loop;

        BGMsource.Play();
		//AudioListener.pause = false;
	}

	// BGM停止
	public void StopBGM(){
		BGMsource.Stop();
		BGMsource.clip = null;
	}

	public void pauseSound() {
		//if (alistener != null)
		//alistener. pause = true;
		ptime = BGMsource.time;
		BGMsource.Pause ();
		//AudioListener.pause = true;
	}

	public void resumeSound() {
		//BGMsource.time = ptime;
		BGMsource.Play();
		BGMsource.time = ptime;
		//if (alistener != null)
		//AudioListener.pause = false;
	}


	// ***** SE再生 *****
	// SE再生
	public void PlaySE(string name, bool loop){
        SoundGroup se = null;
        foreach (SoundGroup sound in SE)
        {
            if (sound.soundName.Equals(name)) {
                se = sound;
                break;
            }
        }
        if ( se == null ){
			return;
		}
		AudioListener.pause = false;

        // 確認音效是否已在播放
        foreach (AudioSource source in SEsources)
        { 
            if (source.clip == se.audioClip && source.isPlaying)
            {
                return;
            }
        }

        // 再生中で無いAudioSouceで鳴らす
        foreach (AudioSource source in SEsources){
			if( false == source.isPlaying ){
				source.clip = se.audioClip;
				if (name == "scoreloop") {
					source.pitch = 2;
				} else {
					source.pitch = 1;
				}
				source.loop = loop;
				source.Play();
				return;
			}
		}  
	}

	// SE停止
	public void StopSE(){
		// 全てのSE用のAudioSouceを停止する
		foreach(AudioSource source in SEsources){
			source.Stop();
			source.loop = false;
			source.clip = null;
		}  
	}
	/*
	// ***** 音声再生 *****
	// 音声再生
	public void PlayVoice(int index){
		if( 0 > index || Voice.Length <= index ){
			return;
		}
		// 再生中で無いAudioSouceで鳴らす
		foreach(AudioSource source in VoiceSources){
			if( false == source.isPlaying ){
				source.clip = Voice[index];
				source.Play();
				return;
			}
		} 
	}

	// 音声停止
	public void StopVoice(){
		// 全ての音声用のAudioSouceを停止する
		foreach(AudioSource source in VoiceSources){
			source.Stop();
			source.clip = null;
		}  
	}
	*/

	/*
	public void PlayingSound(string _soundName){
		//Debug.Log (_soundName);
		AudioSource.PlayClipAtPoint(sound_List[FindSound(_soundName)].audioClip, Camera.main.transform.position);
	}

	public void PlaySoundLoop(string _soundName){
		//GameObject go = GameObject.Find ("GO" + _soundName);
		//if (!go) {
		//	go = new GameObject ("GO" + _soundName);
		//	go.transform.position = Camera.main.transform.position;
		//	source = go.AddComponent<AudioSource>();
		AudioClip clip = sound_List [FindSound (_soundName)].audioClip;
		source.clip = clip;
		source.loop = true;
		//source.Play ();
		//}
		//source.loop = true;
		if(!source.isPlaying)
			source.Play ();
		//source = go.AddComponent<AudioSource>();
		//return source;
		//Destroy(go, clip.length);
	}

	public void stopPlaySoundLoop(){
		//GameObject go = GameObject.Find ("GO" + _soundName);
		//source.gameObject;
		//source.loop = false;
		if(source && source.isPlaying)
			source.Stop ();
		//Destroy (source.gameObject);
	}
	
	private int FindSound(string _soundName){
		int i = 0;
		while( i < sound_List.Count ){
			if(sound_List[i].soundName == _soundName){
				return i;	
			}
			i++;
		}
		return i;
	}
	*/
}
