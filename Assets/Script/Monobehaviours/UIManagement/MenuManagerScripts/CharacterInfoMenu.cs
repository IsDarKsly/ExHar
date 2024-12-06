using UnityEngine;
using TMPro;
public class CharacterInfoMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObject;
    [SerializeField] private TMP_Text n_text;
    [SerializeField] private TMP_Text r_text;
    [SerializeField] private TMP_Text l_text;
    [SerializeField] private TMP_Text c_text;
    [SerializeField] private TMP_Text g_text;

    /// <summary>
    /// Loads basic information about a character into the menu
    /// </summary>
    /// <param name="person"></param>
    public void Activate(Humanoid person) 
    {
        n_text.text = person.Name;
        r_text.text = LocalizationManager.Instance.ReadUIDictionary(person.race.ToString());
        l_text.text = LocalizationManager.Instance.ReadUIDictionary("Level") + person.Level.ToString();
        c_text.text = LocalizationManager.Instance.ReadUIDictionary(person.spec.ToString());
        g_text.text = LocalizationManager.Instance.ReadUIDictionary(person.gender?"Male":"Female");
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates the menu
    /// </summary>
    public void DeActivate() 
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vector2 = MenuManager.Instance.m_position;
        vector2.x -= 100;
        m_gameObject.transform.position = vector2;
    }
}
