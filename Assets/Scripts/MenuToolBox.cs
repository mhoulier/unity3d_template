using System.Collections;
using System.Collections.Generic;
/*
public enum MenuEventType
{
	E_MenuEventNone,
	E_MenuLoadLevel,
	E_MenuQuit,
	E_MenuOpenPanel,
	E_MenuClosePanel,
	E_MenuSendMessage,
};

[System.Serializable]
public class MenuButton
{
	public string m_ButtonLabel = "";
	public MenuEventType m_ButtonEventType = MenuEventType.E_MenuEventNone;
	public int m_ButtonEventParam = 0;
	public string m_ButtonSendMessage = null;
}

[System.Serializable]
public class MenuPanelLayout
{
	public int m_PanelWidth = 200;
	public int m_PanelHeight = 400;
	
	public int m_PanelButtonWidth = 160;
	public int m_PanelButtonHeight = 40;
	public int m_PanelButtonInterspaceY = 60;
	public int m_PanelHeaderHeight = 160;
}

[System.Serializable]
public class MenuPanel
{
	private MenuButton[] m_PanelButtons;
	
	private MenuPanelLayout m_PanelLayout;
	
	private string m_PanelHeaderText = "";
	
	private int m_PanelOffsetX = 0;
	private int m_PanelOffsetY = 0;
	
	
	public MenuPanel(MenuPanelLayout _PanelLayout, string _PanelHeaderText)
	{
		m_PanelLayout = _PanelLayout;	
		m_PanelHeaderText = _PanelHeaderText;
	}
	
	public void SetPanelOffset(int _PanelOffsetX, int _PanelOffsetY)
	{
		m_PanelOffsetX = _PanelOffsetX;
		m_PanelOffsetY = _PanelOffsetY;
	}
}

[System.Serializable]
public class MenuPanelContainer
{
	private List<MenuPanel> m_MenuPanels = null;
	
	private int m_currentPanelIndex = 0;
	
	public MenuPanelContainer()
	{
		m_MenuPanels = new List<MenuPanel>();	
	}
	
	public int AddPanel(MenuPanelLayout _PanelLayout, string _PanelHeaderText)
	{
		MenuPanel newPanel = new MenuPanel(_PanelLayout, _PanelHeaderText);
		
		int newPanelIndex = m_MenuPanels.Count;
		
		m_MenuPanels.Add(newPanel);
		
		return newPanelIndex;
	}
	
	public void SetPanelOffset(int _PanelIndex, int _PanelOffsetX, int _PanelOffsetY)
	{
		System.Diagnostics.Debug.Assert((0 <= _PanelIndex) && (_PanelIndex < m_MenuPanels.Count));
		
		MenuPanel panel = m_MenuPanels[_PanelIndex];
		panel.SetPanelOffset(_PanelOffsetX, _PanelOffsetY);
	}
}
*/
