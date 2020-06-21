using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication13.Helper
{
    public enum Enumstatus
    {
        [Description("Mở")]
        Open = 0,
        [Description("Đóng")]
        In_Progress = 1,
        [Description("Đóng")]
        Close = 4,
        [Description("check_circle_outline")]
        Confirm_True = 5,
        [Description("highlight_off")]
        Confirm_False = 6,
        [Description("Ship COD")]
        SHIP_COD = 7,
        [Description("Thanh Toán Online")]
        TT_Online = 8,
        [Description("Nhận Tại Cửa Hàng")]
        Pick_Up = 9,
        [Description("Chưa Xác Thực")]
        NotConfirm = 10,
        [Description("Đã Xác Thực")]
        Confirmed = 11,
        [Description("Đang Lấy Hàng")]
        DangLayHang = 12,
        [Description("Đang Giao Hàng")]
        DangGiaoHang = 13,
        [Description("Giao Thành Công")]
        DeliverySuccess = 14,
        [Description("Giao Thất Bại")]
        FailToDelivery = 15,
        [Description("Trễ Hạn")]
        Late = 16,
    }
}