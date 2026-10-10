// Mã sinh viên: 202418887
// Họ tên: Hoàng Đình Duy

using System;

public enum DeviceStatus
{
    Active,
    UnderMaintenance,
    Retired
}

public interface INetworkable
{
    string IpAddress { get; }
    bool IsConnected { get; }
    void Connect(string ipAddress);
    void Disconnect();
}

public abstract class Device
{

    public string Id { get; } 
    public string Name { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public DeviceStatus Status { get; set; }

    public Device(string id, string name, int year, decimal price, DeviceStatus status)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã thiết bị không được rỗng.");
            
        if (year > DateTime.Now.Year)
            throw new ArgumentException("Năm sử dụng không được lớn hơn năm hiện tại.");
            
        
        Id = id;
        Name = name;
        Year = year;
        Price = price;
        Status = status;
    }

    public abstract decimal CalculateAnnualMaintenanceCost();

    public override string ToString()
    {
        return $"[{Id}] {Name} - Năm: {Year} - Giá: {Price:N0} VNĐ - Trạng thái: {Status}";
    }
}