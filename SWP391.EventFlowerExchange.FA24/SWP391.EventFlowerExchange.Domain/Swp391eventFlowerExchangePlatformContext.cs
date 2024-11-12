using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.Domain;

public partial class Swp391eventFlowerExchangePlatformContext : IdentityDbContext<Account>
{
    public Swp391eventFlowerExchangePlatformContext()
    {
    }

    public Swp391eventFlowerExchangePlatformContext(DbContextOptions<Swp391eventFlowerExchangePlatformContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<DeliveryLog> DeliveryLogs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ShopNotification> ShopNotifications { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ImageProduct> ImageProducts { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];
        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Optional: Cấu hình tùy chỉnh thêm cho Identity nếu cần
        // Ví dụ về cấu hình khóa chính nếu cần
        // Set Khoa cho IdentityUser
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(iul => new { iul.LoginProvider, iul.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(iur => new { iur.UserId, iur.RoleId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(iut => new { iut.UserId, iut.LoginProvider, iut.Name });
        //modelBuilder.Entity<Account>(entity =>
        //{
        //    entity.HasKey(e => e.UserId).HasName("PK__Account__B9BE370F90DC4CF9");

        //    entity.ToTable("Account");

        //    entity.Property(e => e.UserId).HasColumnName("user_id");
        //    entity.Property(e => e.Balance)
        //        .HasColumnType("decimal(18, 2)")
        //        .HasColumnName("balance");
        //    entity.Property(e => e.CreatedAt)
        //        .HasColumnType("datetime")
        //        .HasColumnName("created_at");
        //    entity.Property(e => e.Email)
        //        .HasMaxLength(255)
        //        .HasColumnName("email");
        //    entity.Property(e => e.Name)
        //        .HasMaxLength(255)
        //        .HasColumnName("name");
        //    entity.Property(e => e.Password)
        //        .HasMaxLength(255)
        //        .HasColumnName("password");
        //    entity.Property(e => e.Phone)
        //        .HasMaxLength(50)
        //        .HasColumnName("phone");
        //    entity.Property(e => e.Role)
        //        .HasMaxLength(50)
        //        .HasColumnName("role");
        //    entity.Property(e => e.Salary).HasColumnName("salary");
        //    entity.Property(e => e.Status).HasColumnName("status");
        //});

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.FollowId).HasName("PK__Follow__15A691443703BF66");

            entity.ToTable("Follow");

            entity.HasIndex(e => new { e.FollowerId, e.SellerId }, "IX_Follow_Unique").IsUnique();

            entity.Property(e => e.FollowId).HasColumnName("follow_id");
            entity.Property(e => e.FollowerId).HasColumnName("follower_id");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");

            entity.HasOne(d => d.Seller).WithMany(p => p.Follows)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Follow__seller_i__395884C4");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK_Messages");

            entity.ToTable("Message");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Contents)
                .HasColumnType("text")
                .HasColumnName("contents");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.ReceiverId)
                .HasMaxLength(450)
                .HasColumnName("receiver_id");
            entity.Property(e => e.SenderId)
                .HasMaxLength(450)
                .HasColumnName("sender_id");

            entity.HasOne(d => d.Notification).WithMany(p => p.Messages)
                .HasForeignKey(d => d.NotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_Notification");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_Account");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.BuyerId }).HasName("PK__Cart__15583D3220AC18D4");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cart_id").ValueGeneratedOnAdd();
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__buyer_id__5AEE82B9");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.ProductId }).HasName("PK__Cart_Ite__6A850DF879A0A29E");

            entity.ToTable("Cart_Item");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_Item__produ__5DCAEF64");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => new { d.CartId, d.BuyerId })
                .HasConstraintName("FK__Cart_Item__5EBF139D");
        });

        modelBuilder.Entity<DeliveryLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Delivery__9E2397E06F1B411E");

            entity.ToTable("Delivery_Log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.TakeOverAt)
                .HasColumnType("datetime")
                .HasColumnName("takeover_at");
            entity.Property(e => e.DeliveryAt)
                .HasColumnType("datetime")
                .HasColumnName("delivery_at");
            entity.Property(e => e.DeliveryPersonId).HasColumnName("delivery_person_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(255)
                .HasColumnName("photo_url");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.DeliveryPerson).WithMany(p => p.DeliveryLogs)
                .HasForeignKey(d => d.DeliveryPersonId)
                .HasConstraintName("FK__Delivery___deliv__534D60F1");

            entity.HasOne(d => d.Order).WithMany(p => p.DeliveryLogs)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Delivery___order__52593CB8");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FE7EA1D64");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__user___4AB81AF0");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__465962296172C64F");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.DeliveredAt)
                .HasColumnName("delivered_at");
            entity.Property(e => e.DeliveryPersonId).HasColumnName("delivery_person_id");
            entity.Property(e => e.IssueReport).HasColumnName("issue_report");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.PhoneNumber)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.VoucherId).HasColumnName("voucher_id");

            entity.HasOne(d => d.Buyer).WithMany(p => p.OrderBuyers)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__Order__buyer_id__3E52440B");

            entity.HasOne(d => d.DeliveryPerson).WithMany(p => p.OrderDeliveryPeople)
                .HasForeignKey(d => d.DeliveryPersonId)
                .HasConstraintName("FK__Order__delivery___403A8C7D");

            entity.HasOne(d => d.Seller).WithMany(p => p.OrderSellers)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Order__seller_id__3F466844");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK_Order_Voucher");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__Order_It__022945F65230A002");

            entity.ToTable("Order_Item");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_Ite__order__4316F928");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_Ite__produ__440B1D61");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF52E9F0063");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ComboType)
                .HasMaxLength(50)
                .HasColumnName("combo_type");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiredAt)
                .HasColumnType("datetime")
                .HasColumnName("expired_at");
            entity.Property(e => e.FreshnessDuration).HasColumnName("freshness_duration");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Product__seller___3B75D760");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__18D3B90FFFABB483");

            entity.ToTable("Request");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RequestType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("request_type");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Request__product__6383C8BA");

            entity.HasOne(d => d.Payment).WithMany(p => p.Requests)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Request_Payment");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Request__user_id__619B8048");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__18D3B90FFFABB483");

            entity.ToTable("Request");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RequestType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("request_type");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Request__product__6383C8BA");

            entity.HasOne(d => d.Payment).WithMany(p => p.Requests)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Request_Payment");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Request__user_id__619B8048");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__60883D90CCF8528A");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__Review__buyer_id__47DBAE45");

            entity.HasOne(d => d.Order).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Review__order_id__46E78A0C");
        });

        modelBuilder.Entity<ShopNotification>(entity =>
        {
            entity.HasKey(e => e.ShopNotificationId).HasName("PK__Shop_Not__F695E7D5C0040F56");

            entity.ToTable("Shop_Notification");

            entity.HasIndex(e => e.FollowerId, "IX_Shop_Notification_follower_id");

            entity.HasIndex(e => e.ProductId, "IX_Shop_Notification_product_id");

            entity.HasIndex(e => e.SellerId, "IX_Shop_Notification_seller_id");

            entity.Property(e => e.ShopNotificationId).HasColumnName("shop_notification_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FollowerId).HasColumnName("follower_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Follower).WithMany(p => p.ShopNotificationFollowers)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.SetNull) // Thay đổi hành vi xóa thành SetNull
                .HasConstraintName("FK_Shop_Notification_Account");

            entity.HasOne(d => d.Product).WithMany(p => p.ShopNotifications)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Shop_Noti__produ__5812160E");

            entity.HasOne(d => d.Seller).WithMany(p => p.ShopNotificationSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.SetNull) // Thay đổi hành vi xóa thành SetNull
                .HasConstraintName("FK_Shop_Notification_Account2");

            entity.HasOne(d => d.Follow).WithMany(p => p.ShopNotifications)
                .HasPrincipalKey(p => new { p.FollowerId, p.SellerId })
                .HasForeignKey(d => new { d.FollowerId, d.SellerId })
                .OnDelete(DeleteBehavior.SetNull) // Thay đổi hành vi xóa thành SetNull
                .HasConstraintName("FK_Shop_Notification_Follow");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AF55D6A98C");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId)
                .HasDefaultValue(0)
                .HasColumnName("order_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("transaction_code");
            entity.Property(e => e.TransactionContent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("transaction_content");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Transacti__order__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__user___4F7CD00D");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.VoucherId).HasColumnName("voucher_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiscountValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("discount_value");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiry_date");
            entity.Property(e => e.MinOrderValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("min_order_value");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EA4938E930");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.PaymentCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("payment_code");
            entity.Property(e => e.PaymentContent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("payment_content");
            entity.Property(e => e.PaymentType).HasColumnName("payment_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Payment)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Payment_Account");
        });

        modelBuilder.Entity<ImageProduct>(entity =>
        {
            entity.ToTable("ImageProduct");

            entity.HasKey(e => e.Id).HasName("PK__ImagePro__3213E83FBC8D53A3");

            entity.Property(e => e.LinkImage).HasMaxLength(450);
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ImageProd__produ__2B0A656D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
