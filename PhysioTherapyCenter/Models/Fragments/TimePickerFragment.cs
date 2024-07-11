using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PhysioTherapyCenter.Models.Fragments
{
    public class TimePickerFragment : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(TimePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<string> _dateSelectedHandler = delegate { };

        private static TimeSpan? Currently;



        public static TimePickerFragment NewInstance(Action<string> OnTimeSet, TimeSpan? currently)
        {
            Currently = currently;
            TimePickerFragment frag = new TimePickerFragment();
            frag._dateSelectedHandler = OnTimeSet;

            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;

            TimePickerDialog dialog = new TimePickerDialog(Activity, this,
           Currently == null ? currently.Hour : Currently.Value.Hours,
           Currently == null ? currently.Minute : Currently.Value.Minutes,
                false);
            return dialog;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            // DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            //  Log.Debug(TAG, selectedDate.ToLongDateString());
            string time = string.Format("{0}:{1}", hourOfDay, minute.ToString().PadLeft(2, '0'));

            _dateSelectedHandler(time);
        }




    }
}