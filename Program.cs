// Mã sinh viên: 202418887
// Họ tên: Hoàng Đình Duy

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Khai báo các biến cục bộ cho toàn chương trình
    static List<LabRoom> rooms = new List<LabRoom>();
    static List<Device> unassignedDevices = new List<Device>();

    static void Main(string[] args)
    {
        InitData();
        
        while (true)
        {
            Console.WriteLine("\n==== HỆ THỐNG QUẢN LÝ THIẾT BỊ ====");
            Console.WriteLine("1. Thêm thiết bị vào phòng");
            Console.WriteLine("2. Tự nhập một thiết bị mới");
            Console.WriteLine("3. In danh sách thiết bị trong từng phòng");
            Console.WriteLine("4. Tính tổng chi phí bảo trì dự kiến của mỗi phòng");
            Console.WriteLine("5. Liệt kê thiết bị cần bảo trì");
            Console.WriteLine("6. Kết nối mạng cho các thiết bị");
            Console.WriteLine("7. Duyệt các thiết bị mạng");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng: ");
            
            string choice = Console.ReadLine();
            Console.WriteLine("-----------------------------------");
            switch (choice)
            {
                case "1": AddDeviceToRoom(); break;
                case "2": AddNewDeviceManually(); break;
                case "3": PrintDevicesInRooms(); break;
                case "4": PrintMaintenanceCosts(); break;
                case "5": PrintMaintenanceRequired(); break;
                case "6": ConnectDevices(); break;
                case "7": BrowseNetworkDevices(); break;
                case "0": return;
                default: Console.WriteLine("Lựa chọn không hợp lệ!"); break;
            }
        }
    }

    static void InitData()
    {
        unassignedDevices.Add(new Computer("PC01", "PC Đồ họa Dell", 2020, 25000000, DeviceStatus.Active, "16GB", "Core i7", true));
        unassignedDevices.Add(new Computer("PC02", "PC Văn phòng HP", 2023, 10000000, DeviceStatus.Active, "8GB", "Core i3", false));
        unassignedDevices.Add(new Printer("PR01", "Canon 2900", 2018, 3000000, DeviceStatus.Active, "Laser", 150000, false));
        unassignedDevices.Add(new NetworkPrinter("PR02", "HP Color LaserJet", 2022, 8000000, DeviceStatus.Active, "Laser", 10000, true));
        unassignedDevices.Add(new Projector("PJ01", "Sony VPL", 2017, 12000000, DeviceStatus.UnderMaintenance, 3500, 3200, "Full HD"));

        rooms.Add(new LabRoom("LAB01", "Phòng TH Máy Tính 1", 30));
        rooms.Add(new LabRoom("LAB02", "Phòng TH Mạng", 25));
        
        Console.WriteLine("Đã khởi tạo dữ liệu mẫu thành công!");
    }

    static void AddDeviceToRoom()
    {
        if (unassignedDevices.Count == 0)
        {
            Console.WriteLine("Không còn thiết bị nào để phân bổ!"); return;
        }

        Console.WriteLine("Chọn phòng (1 - LAB01, 2 - LAB02): ");
        string rChoice = Console.ReadLine();
        LabRoom room = rChoice == "1" ? rooms[0] : (rChoice == "2" ? rooms[1] : null);
        if (room == null) { Console.WriteLine("Lựa chọn phòng không hợp lệ."); return; }

        Console.WriteLine("Danh sách thiết bị chưa phân bổ:");
        for(int i = 0; i < unassignedDevices.Count; i++)
            Console.WriteLine($"{i+1}. {unassignedDevices[i].Name} ({unassignedDevices[i].Id})");
            
        Console.Write("Chọn số thứ tự thiết bị để thêm: ");
        if(int.TryParse(Console.ReadLine(), out int dIndex) && dIndex > 0 && dIndex <= unassignedDevices.Count)
        {
            try {
                var dev = unassignedDevices[dIndex-1];
                room.AddDevice(dev);
                unassignedDevices.Remove(dev);
                Console.WriteLine("=> Thêm thiết bị vào phòng thành công!");
            } catch (Exception ex) {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
    }

    static void AddNewDeviceManually()
    {
        Console.WriteLine("\n--- THÊM THIẾT BỊ MỚI TỪ BÀN PHÍM ---");
        
        Console.Write("Bạn muốn thêm vào phòng nào? (1 - LAB01, 2 - LAB02): ");
        string rChoice = Console.ReadLine();
        LabRoom room = rChoice == "1" ? rooms[0] : (rChoice == "2" ? rooms[1] : null);
        
        if (room == null) 
        { 
            Console.WriteLine("Lựa chọn phòng không hợp lệ."); return; 
        }

        Console.Write("Nhập mã thiết bị (ID): ");
        string id = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("ID không được để trống!"); return;
        }

        if (room.Devices.Any(d => d.Id == id))
        {
            Console.WriteLine($"[LỖI NGHIỆP VỤ]: Mã '{id}' đã tồn tại trong {room.Name}! Dừng nhập liệu.");
            return; 
        }

        Console.WriteLine("-> Mã hợp lệ. Vui lòng nhập tiếp thông tin cho Máy Tính:");
        
        Console.Write("  - Tên thiết bị: ");
        string name = Console.ReadLine();
        
        Console.Write("  - Năm đưa vào sử dụng: ");
        int year;
        while (!int.TryParse(Console.ReadLine(), out year) || year > DateTime.Now.Year)
        {
            Console.Write("    Sai định dạng hoặc > năm nay. Nhập lại năm: ");
        }

        Console.Write("  - Giá mua (VNĐ): ");
        decimal price;
        while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
        {
            Console.Write("    Giá phải lớn hơn 0. Nhập lại: ");
        }

        Console.Write("  - Dung lượng RAM: ");
        string ram = Console.ReadLine();

        Console.Write("  - Bộ vi xử lý: ");
        string cpu = Console.ReadLine();

        Console.Write("  - Có Card đồ họa rời không? (y/n): ");
        bool hasGpu = Console.ReadLine().ToLower() == "y";

        try 
        {
            Device newDev = new Computer(id, name, year, price, DeviceStatus.Active, ram, cpu, hasGpu);
            room.AddDevice(newDev);
            Console.WriteLine($"\n=> ĐÃ THÊM THÀNH CÔNG thiết bị '{name}' vào {room.Name}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi hệ thống: " + ex.Message);
        }
    }

    static void PrintDevicesInRooms()
    {
        Console.WriteLine("\n--- DANH SÁCH THIẾT BỊ TRONG PHÒNG ---");
        foreach (var room in rooms)
        {
            Console.WriteLine($"\n[{room.Name} - Mã: {room.RoomId}]");
            if (room.Devices.Count == 0) Console.WriteLine(" -> Phòng đang trống.");
            foreach (var d in room.Devices)
                Console.WriteLine("   + " + d.ToString());
        }
    }

    static void PrintMaintenanceCosts()
    {
        Console.WriteLine("\n--- TỔNG PHÍ BẢO TRÌ DỰ KIẾN ---");
        foreach (var room in rooms)
        {
            Console.WriteLine($"- {room.Name}: Tổng phí = {room.CalculateAnnualMaintenanceCost():N0} VNĐ");
        }
    }

    static void PrintMaintenanceRequired()
    {
        Console.WriteLine("\n--- DANH SÁCH THIẾT BỊ CẦN BẢO TRÌ ---");
        foreach (var room in rooms)
        {
            var lst = room.GetDevicesRequiringMaintenance();
            foreach (var d in lst)
                Console.WriteLine($" - [{room.Name}] {d.Name} (Mã: {d.Id})");
        }
    }

    static void ConnectDevices()
    {
        Console.WriteLine("\n--- CẤP IP CHO CÁC THIẾT BỊ HỖ TRỢ MẠNG ---");
        bool hasDeviceToConnect = false;

        foreach (var room in rooms)
        {
            foreach (var d in room.Devices)
            {
                if (d is INetworkable netDev && !netDev.IsConnected)
                {
                    hasDeviceToConnect = true;
                    Console.Write($"Nhập IP muốn cấp cho '{d.Name}' (Mã: {d.Id}): ");
                    string ip = Console.ReadLine();
                    
                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        try {
                            netDev.Connect(ip);
                            Console.WriteLine("  -> Kết nối mạng thành công!");
                        } catch(Exception ex) {
                            Console.WriteLine("  -> Lỗi: " + ex.Message);
                        }
                    }
                }
            }
        }

        if (!hasDeviceToConnect) {
            Console.WriteLine("Hiện không có thiết bị nào cần kết nối mạng.");
        }
    }

    static void BrowseNetworkDevices()
    {
        Console.WriteLine("\n--- DANH SÁCH THIẾT BỊ MẠNG ---");
        bool found = false;
        
        foreach (var room in rooms)
        {
            foreach (var d in room.Devices)
            {
                if (d is INetworkable netDev)
                {
                    found = true;
                    string status = netDev.IsConnected ? $"Đang kết nối ({netDev.IpAddress})" : "Ngắt kết nối";
                    Console.WriteLine($" - [{room.Name}] {d.Name} | Trạng thái: {status}");
                }
            }
        }
        
        if(!found) {
            Console.WriteLine("Không tìm thấy bất kỳ thiết bị mạng nào trong các phòng.");
        }
    }
}
