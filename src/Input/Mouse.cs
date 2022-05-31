#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2022 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
#endregion

namespace Microsoft.Xna.Framework.Input
{
	/// <summary>
	/// Allows reading position and button click information from mouse.
	/// </summary>
	public static class Mouse
	{
		#region Public Properties

		public static IntPtr WindowHandle
		{
			get;
			set;
		}

		public static bool IsRelativeMouseModeEXT
		{
			get
			{
				return FNAPlatform.GetRelativeMouseMode();
			}
			set
			{
				FNAPlatform.SetRelativeMouseMode(value);
			}
		}

		#endregion

		#region Internal Variables

		internal static int INTERNAL_WindowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		internal static int INTERNAL_WindowHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
		internal static int INTERNAL_BackBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		internal static int INTERNAL_BackBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

		internal static int INTERNAL_MouseWheel = 0;
		internal static int INTERNAL_MousePositionX = 0;
		internal static int INTERNAL_MousePositionY = 0;
		internal static bool INTERNAL_MouseButtonLeft = false;
		internal static bool INTERNAL_MouseButtonMiddle = false;
		internal static bool INTERNAL_MouseButtonRight = false;
		internal static bool INTERNAL_MouseButtonX1 = false;
		internal static bool INTERNAL_MouseButtonX2 = false;

		#endregion

		#region Public Events

		public static Action<int> ClickedEXT;
		public static Action<int> ReleasedEXT;

		#endregion

		#region Public Interface

		/// <summary>
		/// Gets mouse state information that includes position and button
		/// presses for the provided window
		/// </summary>
		/// <returns>Current state of the mouse.</returns>
		public static MouseState GetState()
		{
			int x = INTERNAL_MousePositionX;
			int y = INTERNAL_MousePositionY;
			ButtonState left = INTERNAL_MouseButtonLeft ? ButtonState.Pressed : ButtonState.Released;
			ButtonState middle = INTERNAL_MouseButtonMiddle ? ButtonState.Pressed : ButtonState.Released;
			ButtonState right = INTERNAL_MouseButtonRight ? ButtonState.Pressed : ButtonState.Released;
			ButtonState x1 = INTERNAL_MouseButtonX1 ? ButtonState.Pressed : ButtonState.Released;
			ButtonState x2 = INTERNAL_MouseButtonX2 ? ButtonState.Pressed : ButtonState.Released;

			// Scale the mouse coordinates for the faux-backbuffer
			x = (int) ((double) x * INTERNAL_BackBufferWidth / INTERNAL_WindowWidth);
			y = (int) ((double) y * INTERNAL_BackBufferHeight / INTERNAL_WindowHeight);

			return new MouseState(
				x,
				y,
				INTERNAL_MouseWheel,
				left,
				middle,
				right,
				x1,
				x2
			);
		}

		/// <summary>
		/// Sets mouse cursor's relative position to game-window.
		/// </summary>
		/// <param name="x">Relative horizontal position of the cursor.</param>
		/// <param name="y">Relative vertical position of the cursor.</param>
		public static void SetPosition(int x, int y)
		{
			// In relative mode, this function is meaningless
			if (IsRelativeMouseModeEXT)
			{
				return;
			}

			// Scale the mouse coordinates for the faux-backbuffer
			x = (int) ((double) x * INTERNAL_WindowWidth / INTERNAL_BackBufferWidth);
			y = (int) ((double) y * INTERNAL_WindowHeight / INTERNAL_BackBufferHeight);

			FNAPlatform.SetMousePosition(WindowHandle, x, y);
		}

		#endregion

		#region Internal Methods

		internal static void INTERNAL_onClicked(int button)
		{
			if (ClickedEXT != null)
			{
				ClickedEXT(button);
			}
		}

		internal static void INTERNAL_onReleased(int button)
		{
			if (ReleasedEXT != null)
			{
				ReleasedEXT(button);
			}
		}

		#endregion
	}
}
