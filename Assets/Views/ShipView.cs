using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Views
{
    public class ShipView : MonoBehaviour, IInteractable
    {
        public void Do()
        {
            Debug.Log("Im explose");
        }
    }
}
