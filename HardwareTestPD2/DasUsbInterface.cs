using System;
using System.Runtime.InteropServices;
namespace DasUsbInterface
{
	/// <summary>
	/// DasLight USB DMX Interface
	/// </summary>
	public class DMXInterface
	{
		/// <summary>
		/// The main interface class.  
		/// </summary>
		/// <param name="universe">The interface id</param>
		/// <exception cref="InterfaceError">Thrown if it cannot connect to the interface</exception>
		public DMXInterface(int universe = 0)
		{
			Universe = universe;
			ReturnCode r = _DasUsbCommand(UsbCommand.Init + Universe * 100, 0, null);
			if (r != ReturnCode.Success)
				throw new InterfaceError(r, "Cannot init.");
			r = _DasUsbCommand(UsbCommand.Open + Universe * 100, 0, null);
			if (r != ReturnCode.Success)
				throw new InterfaceError(r, "Cannot open interface.");
		}
		~DMXInterface()
		{
			_DasUsbCommand(UsbCommand.Close + Universe * 100, 0, null);
			_DasUsbCommand(UsbCommand.Exit + Universe * 100, 0, null);
		}

		/// <summary>
		/// Write channel data to the interface
		/// </summary>
		/// <exception cref="InterfaceError">throws InterfaceError on failure</exception>
		/// <param name="data">Array of channel data
		/// <remarks><c>data</c> is 0-based - array index is DMX Channel - 1</remarks></param>
		/// 
		public void Write(byte[] data)
		{
			ReturnCode r = _DasUsbCommand(UsbCommand.DMXOut + Universe * 100, data.Length, data);
			if (r != ReturnCode.Success)
				throw new InterfaceError(r, "Cannot send data.");
		}

		[DllImport("DasHard2006VB.dll", EntryPoint = "DasUsbCommand")]
		private static extern ReturnCode _DasUsbCommand(UsbCommand command, int param, byte[] data);

		/// <summary>
		/// Send arbitrary commands to interface
		/// </summary>
		/// <param name="command">Command to send</param>
		/// <param name="param">Command parameter</param>
		/// <param name="data">Array of DMX channels</param>
		/// <returns>The code returned by the interface</returns>
		/// <seealso cref="SendCommand_e"/>
		public ReturnCode SendCommand(UsbCommand command, int param, byte[] data)
		{
			return _DasUsbCommand(command + 100 * Universe, param, data);
		}

		/// <summary>
		/// Send arbitrary commands to interface - throws exception on error.
		/// </summary>
		/// <param name="command">Command to send</param>
		/// <param name="param">Command parameter</param>
		/// <param name="data">Array of DMX channels</param>
		/// <exception cref="InterfaceError">Throws InterfaceError on failure</exception>
		/// <seealso cref="SendCommand"/>
		public void SendCommand_e(UsbCommand command, int param, byte[] data)
		{
			ReturnCode r = _DasUsbCommand(command + 100 * Universe, param, data);
			if (r != ReturnCode.Success)
			{
				throw new InterfaceError(r);
			}
		}

		/// <summary>
		/// The universe the interface is assigned to
		/// </summary>
		public int Universe
		{
			get;
			private set;
		}
	}

	/// <summary>
	/// Command codes
	/// </summary>
	public enum UsbCommand
	{
		/// <summary>
		/// Open connnection to interface
		/// </summary>
		Open = 1,
		/// <summary>
		/// Close connection to interface
		/// </summary>
		Close = 2,

		DMXOutOff = 3,

		/// <summary>
		/// send DMX data
		/// </summary>
		DMXOut = 4,
		PortRead = 5,
		PortConfig = 6,
		Version = 7,
		DMXIn = 8,

		/// <summary>
		/// initialise interface
		/// </summary>
		Init = 9,

		/// <summary>
		/// deactivate interface
		/// </summary>
		Exit = 10,
		DMXSCode = 11,
		DMX2Enable = 12,
		DMX2Out = 13,
		Serial = 14,
		Transport = 15,
		DMXEnable = 16,
		DMX3Enable = 17,
		DMX3Out = 18,
		DMX2In = 19,
		DMX3In = 20,


		WriteMemory = 21,
		ReadMemory = 22,
		SizeMemory = 23
	}

	public enum ReturnCode
	{
		Success = 1,
		NothingToDo = 2,
		Error = -1,
		NotOpen = -2,
		AlreadyOpen = -12
	}

	/* TODO: Not sure what to do with this for now.  We probably won't need it.
	struct STIME
	{
		ushort year;
		ushort month;
		ushort dayOfWeek;
		ushort date;
		ushort hour;
		ushort min;
		ushort sec;
		ushort milliseconds;
	}
	*/

	/// <summary>
	/// Thrown by DMXInterface on any error.  Check ReturnCode for information.
	/// </summary>
	public class InterfaceError : Exception
	{
		public InterfaceError(String message)
			: base(message)
		{
		}

		public InterfaceError(ReturnCode errorCode)
			: base()
		{
			ErrorCode = errorCode;
		}

		public InterfaceError(ReturnCode errorCode, String message)
			: base(message)
		{
			ErrorCode = errorCode;
		}

		public ReturnCode ErrorCode
		{
			get;
			private set;
		}
	}
}
