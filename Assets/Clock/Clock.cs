using Model;
using UnityEngine;

namespace Clock
{
    public class Clock : MonoBehaviour
    {
        public Transform secondHand;
        public Transform minuteHand;
        public Transform hourHand;
        
        private void Update()
        {
            var currentTime = GameState.Instance.Time;

            // if (GameState.Instance.Time.Factor > 360)
            // {
            //     secondHand.gameObject.SetActive(false);
            // }
            // else
            // {
            //     secondHand.gameObject.SetActive(true);
            // }

            secondHand.transform.localRotation = Quaternion.Euler(currentTime.Second * -6, 0, 0);
            minuteHand.transform.localRotation = Quaternion.Euler((currentTime.Minute + currentTime.Second/60f) * -6, 0, 0);
            hourHand.transform.localRotation = Quaternion.Euler((currentTime.Hour + currentTime.Minute/60f)* -30, 0, 0);
        }
    }
}