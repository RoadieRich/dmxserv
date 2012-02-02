using System;
using System.Runtime.InteropServices;
namespace DasUsbInterface
{
	public class Interface
	{

		public int Universe
		{
			get;
			private set;
		}

		public Interface(int universe=0)
		{
			Universe = universe;
			if(_DasUsbCommand(UsbCommand.Init + Universe*100, 0, null)!=ReturnCode.None)
				throw new InterfaceError("Cannot init.");
			if(_DasUsbCommand(UsbCommand.Open + Universe*100, 0, null) != ReturnCode.None)
				throw new InterfaceError("Cannot open interface.");
		}
		~Interface()
		{
			_DasUsbCommand(UsbCommand.Close + Universe * 100, 0, null);
			_DasUsbCommand(UsbCommand.Exit + Universe * 100, 0, null);
		}

		public void Write(byte[] data)
		{
			if (_DasUsbCommand(UsbCommand.DMXOut + Universe * 100, data.Length, data) != ReturnCode.None)
				throw new InterfaceError("Cannot send data.");
		}

		[DllImport("DasHard2006VB.dll", EntryPoint = "DasUsbCommand")]
		private static extern ReturnCode _DasUsbCommand(UsbCommand command, int param, byte[] data);
		
		public ReturnCode SendCommand(UsbCommand command, int param, byte[] data)
		{
			return _DasUsbCommand(command+100*Universe, param, data);
		}
	}

	public enum UsbCommand
	{
		Open = 1,
		Close = 2,
		DMXOutOff = 3,
		DMXOut = 4,
		PortRead = 5,
		PortConfig = 6,
		Version = 7,
		DMXIn = 8,
		Init = 9,
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

	public enum DMXInterfaces
	{
		Interface0 = 0,
		Interface1 = 100,
		Interface2 = 200,
		Interface3 = 300,
		Interface4 = 400,
		Interface5 = 500,
		Interface6 = 600,
		Interface7 = 700,
		Interface8 = 800,
		Interface9 = 900
	}

	public enum ReturnCode
	{
		//No error
		None = 1,
		NothingToDo = 2,

		//Errors
		Error = -1,		// Command failed
		NotOpen = -2,
		AlreadyOpen = -12
	}

	// TODO: Not sure what to do with this for now.  We probably won't need it.
	// struct STIME
	//{
	//    ushort year;
	//    ushort month;
	//    ushort dayOfWeek;
	//    ushort date;
	//    ushort hour;
	//    ushort min;
	//    ushort sec;
	//    ushort milliseconds;
	//}

	public class InterfaceError : Exception
	{
		public InterfaceError(String message) : base(message)
		{
		}
	}
}
