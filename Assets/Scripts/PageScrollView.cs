using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this script to the container that has the objects to allow paging
/// </summary>

public class PageScrollView : MonoBehaviour {

    public float m_springStrength = 8.0f;

    private UIScrollView m_scrollView;
    private int m_elementsPerPage;
    private int m_currentScrolledElements;
    private Vector3 m_startingScrollPosition;

    private UIGrid m_grid;

    // Use this for initialization
    void Start () 
    {
        if (m_scrollView == null)
        {
            m_scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
            if (m_scrollView == null)
            {
                Debug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " object in order to work", this);
                enabled = false;
                return;
            }

            m_grid = this.GetComponent<UIGrid>();
            m_elementsPerPage = (int) (m_scrollView.panel.baseClipRegion.z / m_grid.cellWidth);
            m_currentScrolledElements = 0;
            m_startingScrollPosition = m_scrollView.panel.cachedTransform.localPosition;
        }	
    }
    
    // Update is called once per frame
    void Update () {
    
    }
    

    /// <summary>
    /// Scrolls until target position matches target panelAnchorPosition (may be the center of the panel, one of its sides, etc)
    /// </summary>	
    void MoveBy (Vector3 target)
    {
        if (m_scrollView != null && m_scrollView.panel != null)
        {
            // Spring the panel to this calculated position
            SpringPanel.Begin(m_scrollView.panel.cachedGameObject, m_startingScrollPosition - target, m_springStrength);
        }
    }


    public void NextPage()
    {
        if (m_scrollView != null && m_scrollView.panel != null)
        {
            m_currentScrolledElements += m_elementsPerPage;
            if (m_currentScrolledElements > (this.transform.childCount - m_elementsPerPage))
            {
                m_currentScrolledElements = (this.transform.childCount - m_elementsPerPage);
            }
            float nextScroll = m_grid.cellWidth * m_currentScrolledElements;
            Vector3 target = new Vector3(nextScroll, 0.0f, 0.0f);				
            MoveBy(target);
        }
    }

    public void PreviousPage()
    {
        if (m_scrollView != null && m_scrollView.panel != null)
        {
            m_currentScrolledElements -= m_elementsPerPage;
            if (m_currentScrolledElements < 0)
            {
                m_currentScrolledElements = 0;
            }
            float nextScroll = m_grid.cellWidth * m_currentScrolledElements;
            Vector3 target = new Vector3(nextScroll, 0.0f, 0.0f);				
            MoveBy(target);
        }
    }

}