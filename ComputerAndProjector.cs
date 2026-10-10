// Mã sinh viên: 202418887
// Họ tên: Hoàng Đình Duy

using System;

public class Computer : Device, INetworkable
{
    public string Ram { get; set; }
    public string Processor { get; set; }
    public bool HasDiscreteGPU { get; set; }

    public string IpAddress { get; private set; }
    public bool IsConnected { get; private set; }

    public Computer(string id, string name, int year, decimal price, DeviceStatus status, 
                    string ram, string processor, bool hasDiscreteGPU) 
        : base(id, name, year, price, status)
    {
        Ram = ram;
        Processor = processor;
        HasDiscreteGPU = hasDiscreteGPU;
    }

    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = Price * 0.05m;
        if (HasDiscreteGPU) cost += Price * 0.02m;
        if (DateTime.Now.Year - Year > 5) cost += Price * 0.01m;
        return cost;
    }

    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("IP không được rỗng.");
        if (IsConnected)
            throw new InvalidOperationException("Thiết bị đang kết nối. Không thể kết nối lại.");
        
        IpAddress = ipAddress;
        IsConnected = true;
    }

    public void Disconnect()
    {
        IsConnected = false;
        IpAddress = null; // Mất IP khi ngắt kết nối
    }

    public override string ToString() => base.ToString() + $" - PC (RAM: {Ram}, GPU rời: {HasDiscreteGPU}, Mạng: {(IsConnected ? IpAddress : "Không kết nối")})";
}

public class Projector : Device
{
    public int Brightness { get; set; } // Lumen
    public int LampHours { get; set; }
    
    // Thuộc tính tự bổ sung: Độ phân giải
    public string Resolution { get; set; } 

    public Projector(string id, string name, int year, decimal price, DeviceStatus status, 
                     int brightness, int lampHours, string resolution) 
        : base(id, name, year, price, status)
    {
        Brightness = brightness;
        LampHours = lampHours;
        Resolution = resolution;
    }

    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = Price * 0.03m;
        if (LampHours > 3000) cost += 1500000m;
        return cost;
    }
    
    public override string ToString() => base.ToString() + $" - Máy chiếu ({Brightness}lm, {LampHours}h, {Resolution})";
}
