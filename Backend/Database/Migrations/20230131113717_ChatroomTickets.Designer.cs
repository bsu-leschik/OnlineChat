﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(ChatDatabase))]
    [Migration("20230131113717_ChatroomTickets")]
    partial class ChatroomTickets
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AdministratorsUser", b =>
                {
                    b.Property<Guid>("AdministratorsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ModeratorsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdministratorsId", "ModeratorsId");

                    b.HasIndex("ModeratorsId");

                    b.ToTable("AdministratorsUser");
                });

            modelBuilder.Entity("Entities.ChatroomTicket", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LastMessageRead")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ChatroomId");

                    b.HasIndex("ChatroomId");

                    b.ToTable("ChatroomTicket");
                });

            modelBuilder.Entity("Entities.Chatrooms.Administrators", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PublicChatroomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PublicChatroomId")
                        .IsUnique();

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("Entities.Chatrooms.Chatroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastMessageTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Chatroom");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChatroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SendingTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChatroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Token")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Entities.Chatrooms.PrivateChatroom", b =>
                {
                    b.HasBaseType("Entities.Chatrooms.Chatroom");

                    b.ToTable("PrivateChatroom");
                });

            modelBuilder.Entity("Entities.Chatrooms.PublicChatroom", b =>
                {
                    b.HasBaseType("Entities.Chatrooms.Chatroom");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("PublicChatroom");
                });

            modelBuilder.Entity("AdministratorsUser", b =>
                {
                    b.HasOne("Entities.Chatrooms.Administrators", null)
                        .WithMany()
                        .HasForeignKey("AdministratorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.User", null)
                        .WithMany()
                        .HasForeignKey("ModeratorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.ChatroomTicket", b =>
                {
                    b.HasOne("Entities.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("UserTickets")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Entities.User", "User")
                        .WithMany("ChatroomTickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Chatrooms.Administrators", b =>
                {
                    b.HasOne("Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Entities.Chatrooms.PublicChatroom", "PublicChatroom")
                        .WithOne("Administrators")
                        .HasForeignKey("Entities.Chatrooms.Administrators", "PublicChatroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("PublicChatroom");
                });

            modelBuilder.Entity("Entities.Message", b =>
                {
                    b.HasOne("Entities.Chatrooms.Chatroom", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatroomId");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.HasOne("Entities.Chatrooms.Chatroom", null)
                        .WithMany()
                        .HasForeignKey("ChatroomId");
                });

            modelBuilder.Entity("Entities.Chatrooms.PrivateChatroom", b =>
                {
                    b.HasOne("Entities.Chatrooms.Chatroom", null)
                        .WithOne()
                        .HasForeignKey("Entities.Chatrooms.PrivateChatroom", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Chatrooms.PublicChatroom", b =>
                {
                    b.HasOne("Entities.Chatrooms.Chatroom", null)
                        .WithOne()
                        .HasForeignKey("Entities.Chatrooms.PublicChatroom", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Chatrooms.Chatroom", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UserTickets");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Navigation("ChatroomTickets");
                });

            modelBuilder.Entity("Entities.Chatrooms.PublicChatroom", b =>
                {
                    b.Navigation("Administrators")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
