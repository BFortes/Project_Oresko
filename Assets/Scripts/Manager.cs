using UnityEngine;
using System.Collections;
using System;

public class Manager : MonoBehaviour {
  
  [System.Serializable]
  public class MySound {

    public AudioClip  m_song;
    public string     m_name;
  }

  public Texture2D[] m_bgList;
  public MySound[]   m_soundsList;

  // ---

  public UITexture m_background;
  private int      m_bgIndex;

  public UIScrollView m_scrollView;
  public UIGrid       m_grid;
  public GameObject   m_buttonPrefab;

  bool m_isPlayingSound;

  AudioSource m_sndSource;

  void Awake () {
  
    //UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;

    int lastIndex = PlayerPrefs.GetInt("BGIndex", 0);

    m_bgIndex = UnityEngine.Random.Range(0, m_bgList.Length);

    while(m_bgIndex == lastIndex)
      m_bgIndex = UnityEngine.Random.Range(0, m_bgList.Length);

    Debug.Log(m_bgIndex);

    m_background.mainTexture = m_bgList[m_bgIndex];
  }

	// Use this for initialization
	void Start () {
	  
    m_sndSource        = gameObject.AddComponent<AudioSource>();
    m_sndSource.clip   = null;
    m_sndSource.loop   = false;
    m_sndSource.volume = 1f;

    m_isPlayingSound = false;

    FillButtonsOnGrid();
	}
  
  void PlaySound ( int index ) {
    
    if(m_isPlayingSound)
      m_sndSource.Stop();

      m_sndSource.clip = m_soundsList[index].m_song;

      m_isPlayingSound = true;

      m_sndSource.Play(); 
  }
  	
	void LateUpdate () {
	  
    if(m_isPlayingSound) {
    
      if(m_sndSource.clip != null) {
      
        if(!m_sndSource.isPlaying) {
        
          m_isPlayingSound = false;

          m_sndSource.clip = null;
        }
      }
    }
	}

  void Update () {
  
    if ((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.WindowsEditor)) {

      if (Input.GetKeyDown(KeyCode.Escape)) {
        
        Application.Quit();
        return;
      }
    }
  }

  void FillButtonsOnGrid () {

    int index = 0;

    foreach(MySound song in m_soundsList) {
    
      Transform newItem = NGUITools.AddChild(m_grid.gameObject, m_buttonPrefab.gameObject).transform;

      newItem.name = "Button_" + index.ToString("D3");

      newItem.localPosition = new Vector3(m_buttonPrefab.transform.localPosition.x, m_buttonPrefab.transform.localPosition.y, 1.0f);

      newItem.GetComponentInChildren<UILabel>().text = song.m_name;

      EventDelegate.Add(newItem.transform.GetComponent<UIButton>().onClick, ButtonPlaySound);

      index++;
    }

    m_grid.Reposition();
  }

  public void ButtonPlaySound () {
    
    int index = Convert.ToInt32( UIButton.current.name.Split('_')[1] );

    Debug.Log(">>> " + m_soundsList[index].m_name);

    PlaySound(index);
  }

  public void ButtonPlayRandom () {
    
    int index = UnityEngine.Random.Range(0, m_soundsList.Length);

    Debug.Log(">>> " + m_soundsList[index].m_name);

    PlaySound(index);
  }

  void OnApplicationQuit() {

		PlayerPrefs.SetInt("BGIndex", m_bgIndex);
	}
}
