using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPSepController
{
    public class PlayerInput : PlayerComponent
    {
        [Header("All Inputs")]
        [Tooltip("This list should include all Axis-based inputs. A good example is 'Horizontal', which outputs -1 or 1 if you press [a] or [d] respectively.")]
        public List<PlayerInputAxis> allAxis = new List<PlayerInputAxis>();
        [Tooltip("This list should include all Button-based inputs. e.g. 'Jump' will activate when pressing [Space].")]
        public List<PlayerInputButton> allButtons = new List<PlayerInputButton>();


        public override void OnPlayerUpdate()
        {
            DoInputs_Axis();
            DoInputs_Button();
        }


        void DoInputs_Axis()
        {
            foreach (PlayerInputAxis axis in allAxis)
            {
                float oldValue = axis.value;                        //Get the value before updating so we can see if it changed

                //Apply the new value based, keeping in mind if it shoud use Raw values.
                if (axis.useRaw)
                    axis.value = Input.GetAxisRaw(axis.name);
                else
                    axis.value = Input.GetAxis(axis.name);

                //If the axis' value has changed, either event must be called.
                if (axis.value != oldValue)
                {
                    if (axis.value == 0)
                        axis.onZero?.Invoke();                      //Axis has reverted back to zero
                    else
                        axis.onMove?.Invoke();                      //Axis has gone from zero to a different value
                }
            }
        }


        void DoInputs_Button()
        {
            foreach (PlayerInputButton button in allButtons)
            {
                bool oldValue = button.isPressed;                   //Get the value before updating so we can see if it changed.
                button.isPressed = Input.GetButton(button.name);    //Apply new value                          


                //If the button's value has changed, either event must be called.
                if (oldValue != button.isPressed)
                {
                    if (button.isPressed)
                        button.onButtonDown?.Invoke();            //Button is pressed now and wasn't before --> Invoke Button Enter                    
                    else
                        button.onButtonUp?.Invoke();              //Button isn't pressed but was before --> Invoke Button Exit                                                                    
                }
            }
        }



        public float GetInputAxisValue(int _index)
        {
            try
            { return allAxis[_index].value; }
            catch
            {
                Debug.LogError($"ERROR: Axis with index '{_index}' couldn't be found. Returning 0.", this.gameObject);
                return 0;
            }
        }
        public bool GetInputButtonValue(int _index)
        {
            try
            { return allButtons[_index].isPressed; }
            catch
            {
                Debug.LogError($"ERROR: Button with index '{_index}' couldn't be found. Returning false", this.gameObject);
                return false;
            }
        }
    }




    [Serializable]
    public class PlayerInputAxis
    {
        [Tooltip("The name of the Input Axis. Should be the same as in the Project Settings/Inputs")]
        public string name = "Horizontal";
        [Tooltip("Use Raw values?\nNote: This means numbers are only -1, 0 or 1. No smoothing.")]
        public bool useRaw = true;
        [HideInInspector] public float value;

        [HideInInspector] public Action onMove = null;
        [HideInInspector] public Action onZero = null;
    }

    [Serializable]
    public class PlayerInputButton
    {
        [Tooltip("The name of the Input Bool. Should be the same as in the Project Settings/Inputs")]
        public string name = "Jump";
        [HideInInspector] public bool isPressed = false;

        [HideInInspector] public Action onButtonDown = null;
        [HideInInspector] public Action onButtonUp = null;
    }
}