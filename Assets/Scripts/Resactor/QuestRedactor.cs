using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class QuestRedactor : MonoBehaviour
{
    [SerializeField] private TextAsset _textAsset;
    [SerializeField] private Text _debugInfo;

    [SerializeField] private InputField _idLocation;
    [SerializeField] private InputField _nameLocation;
    [SerializeField] private InputField _descriptionLocation;
    [SerializeField] private AnswerGO[] _answers;
    [SerializeField] private InputField[] _items;
    [SerializeField] private InputField[] _persons;

    private List<Location> _locations;

    private void Start()
    {
        Load();
    }


    public void AddLocation()
    {
        int id = int.Parse(_idLocation.text);

        var location = _locations.FirstOrDefault(l => l.Id == id);

        var answers = _answers
          .Where(a => !string.IsNullOrEmpty(a.Text.text))
          .Select(a => new Answer
          {
              Text = a.Text.text,
              NextLocationId = a.GetNextLocationId()
          })
          .ToList();

        var persons = _persons
            .Where(p => !string.IsNullOrEmpty(p.text))
            .Select(p => new DialogObject
            {
                Name = p.text,
            }).ToList();

        var items = _items
            .Where(i => !string.IsNullOrEmpty(i.text))
            .Select(i => new Item
            {
                Name = i.text,
            }).ToList();

        if (location == null)
        {
            location = new Location
            {
                Id = id,
                Name = _nameLocation.text,
                Info = _descriptionLocation.text.Split("*"),
                Objects = persons,
            };

            _locations.Add(location);
        }
        else
        {
            location.Id = id;
            location.Name = _nameLocation.text;
            location.Info = _descriptionLocation.text.Split("*");
            location.Objects = persons;
        }

        _debugInfo.text = $"Added location {id}";

        SaveAsset();
    }
    public void Load()
    {

        var json = Resources.Load<TextAsset>("locations");


        if (_locations == null)
        {
            _locations = new();
            _locations = JsonConvert.DeserializeObject<List<Location>>(json.text);
        }
        else if (_idLocation.text != string.Empty)
        {
            var id = int.Parse(_idLocation.text);

            Location loc = _locations.FirstOrDefault(l => l.Id == id);
            if (loc != null)
                ApplyLocate(loc);
            else _debugInfo.text = "Такой локации нет";
        }
    }

    private void ApplyLocate(Location loc)
    {
        ClearAllField();

        if (loc.Objects != null)
        {
            var cnt = 0;
            foreach (var person in loc.Objects)
            {
                _persons[cnt++].text = person.Name;
            }
        }


        _nameLocation.text = loc.Name;
        _descriptionLocation.text = loc.Info.ToString();

    }

    private void ClearAllField()
    {
        foreach (var answ in _answers)
        {
            answ.Clear();
        }
        foreach (var p in _persons)
        {
            p.text = string.Empty;
        }
        foreach (var i in _items)
        {
            i.text = string.Empty;
        }
    }

    public void SaveAsset()
    {
        string json = JsonConvert.SerializeObject(_locations);
        // Получаем путь к директории со скриптом
        string scriptPath = Path.GetDirectoryName(Path.GetFullPath(Application.dataPath));
        // Добавляем имя файла
        string fullPath = Path.Combine(scriptPath, "Assets/Resources/locations.json");
        // Записываем строку в ресурс по указанному пути
        File.WriteAllText(fullPath, json);
    }
}
