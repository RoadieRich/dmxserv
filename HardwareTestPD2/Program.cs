using System;
using System.Threading;
using DasUsbInterface;
using DasUsbInterface.Stub;

using DMXInterface = DasUsbInterface.Stub.DMXInterfaceStub;
namespace HardwareTestPD2
{
	class Program
	{
		private static DMXInterface Interface;
	
		static void Main(string[] args)
		{
			int i;
			
			byte[] dmxblock = new byte[512];
			dmxblock.Initialize();
			
			for (i = 0; i < 13; i++)
			{
				dmxblock[i] = 255;
			}

			//init
			Console.WriteLine("Initialising...");
			try
			{
				Interface = new DMXInterfaceStub();
			}
			catch (InterfaceError e)
			{
				Console.WriteLine(e.Message);
				Console.ReadKey();
				return;
			}

			if (Interface != null)
			{
				Console.WriteLine("Sending test data.");
				for (byte val = 255; val > 0; val--)
				{
					try
					{
						Interface.Write(dmxblock);
					}
					catch (InterfaceError e)
					{
						Console.WriteLine(e.Message);
					}
					for (int j = 0; j < 13; j++)
					{
						dmxblock[j] = val;
					}
					Thread.Sleep(10);
				}

				Console.WriteLine("Closing...");
				Interface = null;
			}
			Console.WriteLine("Done. Press any key to continue.");
			Console.ReadKey();
		}
	}
}
