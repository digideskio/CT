using UnityEngine;
using System.Collections;
using System;
using InControl;

namespace CustomProfileExample
{

	public class KeyboardController : CustomInputDeviceProfile {
		public KeyboardController () {
		// Use this for initialization
			Name = "Keyboard";
			Meta = "A keyboard control scheme for controlling a hover car";

			SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			LowerDeadZone = 0.0f;
			UpperDeadZone = 1.0f;

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Interact",
					Target = InputControlType.Action1,
					Source = KeyCodeButton(KeyCode.I)
				},

				new InputControlMapping {
					Handle = "Mode",
					Target = InputControlType.Action2,
					Source = KeyCodeButton(KeyCode.O)
				},

				new InputControlMapping {
					Handle = "Engage",
					Target = InputControlType.LeftTrigger,
					Source = KeyCodeButton(KeyCode.Space)
				},

				new InputControlMapping {
					Handle = "Thrust",
					Target = InputControlType.RightTrigger,
					Source = KeyCodeButton(KeyCode.K)				
				},

				new InputControlMapping {
					Handle = "Inventory",
					Target = InputControlType.Select,
					Source = KeyCodeButton(KeyCode.Tab)
				}
			};

			AnalogMappings = new[]
			{
				new InputControlMapping {
					Handle = "Strafe Left",
					Target = InputControlType.LeftStickLeft,
					Source = KeyCodeButton(KeyCode.A)
				},

				new InputControlMapping {
					Handle = "Strafe Right",
					Target = InputControlType.LeftStickRight,
					Source = KeyCodeButton(KeyCode.D)
				},

				new InputControlMapping {
					Handle = "Rotate Left",
					Target = InputControlType.RightStickLeft,
					Source = KeyCodeButton(KeyCode.J)
				},

				new InputControlMapping {
					Handle = "Rotate Right",
					Target = InputControlType.RightStickRight,
					Source = KeyCodeButton(KeyCode.L)
				},

				new InputControlMapping {
					Handle = "Move Forward",
					Target = InputControlType.LeftStickUp,
					Source = KeyCodeButton(KeyCode.W)
				},

				new InputControlMapping {
					Handle = "Move Backward",
					Target = InputControlType.LeftStickDown,
					Source = KeyCodeButton(KeyCode.S)
				},
			};
		}	
	}
}