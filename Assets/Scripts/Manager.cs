using UnityEngine;
using System.Collections;
using System;

public class Manager : MonoBehaviour {
  
  [System.Serializable]
  public class Song {

    public AudioClip  m_song;
    public string     m_name;
  }

  public Song[]   m_soundsList;

  // ---

  public UIScrollView m_scrollView;
  public UIGrid       m_grid;
  public GameObject   m_buttonPrefab;

  bool m_isPlayingSound;

  AudioSource m_sndSource;

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

  void FillButtonsOnGrid () {

    int index = 0;

    foreach(Song song in m_soundsList) {
    
      Transform newItem = NGUITools.AddChild(m_grid.gameObject, m_buttonPrefab.gameObject).transform;

      newItem.name = "Button_" + index.ToString("D2");

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
}
