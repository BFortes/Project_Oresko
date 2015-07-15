using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
  
  enum ScoreState { Null, Letter, Light, Star, Total };
  
  ScoreState m_state = ScoreState.Null;

  bool m_isPlaying = false;

  float m_timer = -1f;

  int m_numStars = 0;

  float m_rt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
    float dt = Time.deltaTime;

    float rt = Time.time;

    if(!m_isPlaying) {
    
      if(Input.GetKeyUp(KeyCode.P)) {
        
        m_rt = rt;

        m_numStars = Random.Range(1, 11);

        Debug.Log("stars: " + m_numStars);

        m_timer = 1f;

        m_isPlaying = true;

        Debug.Log("LETTER " + (rt - m_rt) );

        m_state = ScoreState.Letter;  
      }
    }

    switch (m_state) {
    
      case ScoreState.Null: {
      }
      break;
      case ScoreState.Letter: {
        
        if(m_timer >= 0f) {
        
          m_timer -= dt;

          if(m_timer < 0f) {
            
            //m_hasNext = true;

            Debug.Log("STAR " + (rt - m_rt));

            m_numStars--;

            m_timer = (m_numStars > 0) ? 0.5f : 1f;

            m_state = ScoreState.Star;
          }
        }

      }
      break;
      case ScoreState.Light: {
      }
      break;
      case ScoreState.Star: {

        if(m_timer >= 0f) {
        
          m_timer -= dt;

          if(m_timer < 0f) {
            
            if(m_numStars > 0) {
              
              m_numStars--;

              Debug.Log("STAR " + (rt - m_rt));

              m_timer = (m_numStars > 0) ? 0.5f : 1f;
            }
            else {
              
              Debug.Log("TOTAL " + (rt - m_rt) );
              
              m_timer = 1f;

              m_state = ScoreState.Total;
            }
          }
        } 
        
      }
      break;
      case ScoreState.Total: {
      }
      break;
    }
	}

}
