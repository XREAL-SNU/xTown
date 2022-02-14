using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;


public class CalendarController : MonoBehaviour
{
    CultureInfo _culture = new CultureInfo("en-US");
 
    public Text _monthYearText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController _calendarInstance;

    void Start()
    {
        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 39  + startPos.x, startPos.y - (i / 7) * 30, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now.AddDays(-1);

        CreateCalendar();

    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _monthYearText.text = _dateTime.ToString("MMM", _culture).ToUpper() + " " + _dateTime.Year.ToString();
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }



    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }




    //Item 클릭했을 경우 Text에 표시.
    public void OnDateItemClick(string day)
    {
    
    }
}
