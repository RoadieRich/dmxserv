﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using DasUsbInterface;

namespace HardwareTestPD2
{
	class Program
	{
		public static Boolean DoCommand(UsbCommand command, int param, byte[] data)
		{
			throw new NotImplementedException();
		}

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
			Interface inter = null;
			try
			{
				inter = new Interface();
			}
			catch (InterfaceError e)
			{
				Console.WriteLine(e.Message);
			}

			if (inter != null)
			{
				Console.WriteLine("Sending test data.");
				for (byte val = 255; val >= 0; val >>= 1)
				{
					try
					{
						inter.Write(dmxblock);
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
				inter = null;
			}
			Console.WriteLine("Done. Press any key to continue.");
			Console.ReadKey();
		}
	}
}