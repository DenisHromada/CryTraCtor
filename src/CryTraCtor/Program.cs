using System.Configuration;
using CryTraCtor.PacketArrivalHandler;
using SharpPcap;
using SharpPcap.LibPcap;

var captureFilePath = ConfigurationManager.AppSettings["CaptureFilePath"];

using var device = new CaptureFileReaderDevice(captureFilePath);
device.Open();
device.OnPacketArrival += PacketArrivalHandler.HandlePacketArrival;
device.Capture();
