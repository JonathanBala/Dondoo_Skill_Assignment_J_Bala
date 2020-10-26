/************************************************************************************
Copyright : None

Developer : Jonathan de Canha Bala

Script Description : 
    This script showcases the use of polymorphism and inheritance by simply debugging the results in the unity debug consol.
************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeXzXBlitZ {
    // Human is a base class where derived classes (Father and son) can inherit functions and variables from
    public class Human {
        public string firstName = "First Name";
        public string lastName = "Last Name";

        // void is virtual so that it can be overriden in derived classes
        public virtual void PrintDetails () {
            Debug.Log ($" Human name is {firstName} {lastName}");
        }
    }

    public class Father : Human {
        public override void PrintDetails () {
            Debug.Log ($" Father name is {firstName} {lastName}");
        }
    }

    public class Son : Human {
        public override void PrintDetails () {
            Debug.Log ($" Son name is {firstName} {lastName}");
        }
    }

    public class InheritanceDemonstration : MonoBehaviour {
        [Header ("Names to assign")]
        [SerializeField] string fathersName;
        [SerializeField] string sonsName;
        [SerializeField] string lastName;
        List<Human> humans = new List<Human> ();
        private void Start () {
            // Add a new father and son to the list and assign there names.
            humans.Add (new Father ());
            humans.Add (new Son ());
            humans[0].firstName = fathersName;
            humans[1].firstName = sonsName;
            foreach (var human in humans) {
                human.lastName = lastName;
                human.PrintDetails ();
            }
        }
    }
}