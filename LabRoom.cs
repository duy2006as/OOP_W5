using System;
using System.Collections.Generic;
using System.Linq;

public class LabRoom
{
    public string RoomId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public List<Device> Devices { get; private set; }

    public LabRoom(string roomId, string name, int capacity)
    {
        RoomId = roomId;
        Name = name;
        Capacity = capacity;
        Devices = new List<Device>();
    }

    public void AddDevice(Device device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
            
        if (Devices.Count >= Capacity)
            throw new InvalidOperationException("Phòng đã đạt sức chứa tối đa.");

        if (Devices.Any(d => d.Id == device.Id))
            throw new ArgumentException($"Thiết bị mã {device.Id} đã tồn tại trong phòng.");

        Devices.Add(device);
    }

    public bool RemoveDevice(string deviceId)
    {
        var device = FindDevice(deviceId);
        if (device != null)
        {
            return Devices.Remove(device);
        }
        return false;
    }

    public Device FindDevice(string deviceId)
    {
        return Devices.FirstOrDefault(d => d.Id == deviceId);
    }

        public decimal CalculateAnnualMaintenanceCost()
    {
        decimal total = 0;
        foreach (var d in Devices)
        {
            // Xóa hết if-else. Nhờ Tính đa hình, chỉ cần 1 dòng code duy nhất:
            total += d.CalculateAnnualMaintenanceCost(); 
        }
        return total;
    }

   public List<Device> GetDevicesRequiringMaintenance()
    {
        List<Device> result = new List<Device>();
        
        foreach (var d in Devices)
        {
            if (d.Status == DeviceStatus.UnderMaintenance)
            {
                result.Add(d);
            }
        }
        
        foreach (var d in Devices)
        {
            if ((DateTime.Now.Year - d.Year) > 5)
            {
                result.Add(d);
            }
        }
        
        return result;
    }
}
