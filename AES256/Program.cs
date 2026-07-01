
/// GET BALANCE
/// 

//using System;
//using System.Security.Cryptography;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "2778f49ea5338e0f84535d37a78b63fc";
//        string iv = "ebf582f617150b3e";

//        // JSON Request Body
//        string requestBody = @"{
//            ""Request"": {
//                ""Header"": {
//                    ""Timestamp"": ""20250125113024983"",
//                    ""ChannelDetails"": {
//                        ""ChannelID"": ""XPRESS"",
//                        ""ChannelType"": ""WEB"",
//                         ""ChannelSubClass"": """",
//                         ""BranchCode"": """",
//                         ""ChannelCusHdr"": {
//                         ""ChannelProtocol"": """"
//                         }
//                    },
//                    ""DeviceDetails"": {
//                        ""DeviceID"": """",
//                        ""IMEINumber"": """",
//                        ""ClientIP"": """",
//                        ""OS"": """",
//                        ""BrowserType"": """",
//                        ""MobileNumber"": """",
//                        ""GeoLocation"": {
//                            ""Latitude"": """",
//                            ""Longitude"": """"
//                        }
//                    }
//                },
//                ""Body"": {
//                    ""SenderCD"": ""XPRESS_EX_WS"",
//                    ""TransactionID"": ""100000000000"",
//                    ""TransactionDate"": ""20190930"",
//                    ""Signature"": """",
//                    ""CheckSum"": ""60-45-BD-14-8D-99""
//                }
//            }
//        }";

//        string encryptedRequest = Encrypt(requestBody, key, iv);
//        Console.WriteLine("Encrypted Request Body (Hex):");
//        Console.WriteLine(encryptedRequest);
//    }

//    static string Encrypt(string plainText, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform encryptor = aes.CreateEncryptor())
//            {
//                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
//            }
//        }
//    }
//}


/// ACCOUNT INQUIRY
/// 

//using System;
//using System.Security.Cryptography;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "2778f49ea5338e0f84535d37a78b63fc";
//        string iv = "ebf582f617150b3e";

//        // JSON Request Body
//        string requestBody = @"{
//          ""Request"": {
//            ""Header"": {
//              ""Timestamp"": ""20250120150624983"",
//              ""ChannelDetails"": {
//                ""ChannelID"": ""XPRESS"",
//                ""ChannelType"": ""WEB"",
//                ""ChannelSubClass"": """",
//                ""BranchCode"": """",
//                ""ChannelCusHdr"": {
//                  ""ChannelProtocol"": """"
//                }
//              },
//              ""DeviceDetails"": {
//                ""DeviceID"": """",
//                ""IMEINumber"": """",
//                ""ClientIP"": """",
//                ""OS"": """",
//                ""BrowserType"": """",
//                ""MobileNumber"": """",
//                ""GeoLocation"": {
//                  ""Latitude"": """",
//                  ""Longitude"": """"
//                }
//              }
//            },
//            ""Body"": {
//              ""SenderCD"": ""XPRESS_EX_WS"",
//              ""TransactionID"": ""1000000000001"",
//              ""TransactionDate"": ""20250130"",
//              ""BeneficiaryAccNumber"": ""0002053000010425"",
//              ""Signature"": """",
//              ""CheckSum"": ""60-45-BD-14-8D-99""
//            }
//          }
//        }";

//        string encryptedRequest = Encrypt(requestBody, key, iv);
//        Console.WriteLine("Encrypted Request Body (Hex):");
//        Console.WriteLine(encryptedRequest);
//    }

//    static string Encrypt(string plainText, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform encryptor = aes.CreateEncryptor())
//            {
//                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
//            }
//        }
//    }
//}


/// SELF BANK TRANSFER
/// 

//using System;
//using System.Security.Cryptography;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "2778f49ea5338e0f84535d37a78b63fc";
//        string iv = "ebf582f617150b3e";

//        // JSON Request Body
//        string requestBody = @"{
//          ""Request"": {
//            ""Header"": {
//              ""Timestamp"": ""20250121115924983"",
//              ""ChannelDetails"": {
//                ""ChannelID"": ""XPRESS"",
//                ""ChannelType"": ""WEB"",
//                ""ChannelSubClass"": ""Retail"",
//                ""BranchCode"": """",
//                ""ChannelCusHdr"": {
//                  ""ChannelProtocol"": ""Mobile""
//                }
//              },
//              ""DeviceDetails"": {
//                ""DeviceID"": ""Device1"",
//                ""IMEINumber"": """",
//                ""ClientIP"": """",
//                ""OS"": """",
//                ""BrowserType"": """",
//                ""MobileNumber"": """",
//                ""GeoLocation"": {
//                  ""Latitude"": ""13.072090"",
//                  ""Longitude"": ""80.201859""
//                }
//              }
//            },
//            ""Body"": {
//              ""SenderCD"": ""XPRESS_EX_WS"",
//              ""TransactionID"": ""5183852741912"",
//              ""TransactionAmount"": ""1000"",
//              ""TransactionDate"": ""20250220"",
//              ""RemitterName"": ""Nilesh Katkar"",
//              ""RemitterAddress1"": ""Kerala"",
//              ""RemitterAddress2"": "" "",
//              ""RemitterAddress3"": """",
//              ""RemitterAddress4"": """",
//              ""BeneficiaryAccNo"": ""0002053000010425"",
//              ""BeneficiaryAccName"": ""Benna Denny"",
//              ""CountryCD"": ""IN"",
//              ""PurposeCD"": ""P1301"",
//              ""NotificationType"": ""SMS"",
//              ""NotificationValue"": ""4748827318"",
//              ""Signature"": """",
//              ""CheckSum"": ""60-45-BD-14-8D-99"",
//              ""AdditionalField"": """"
//            }
//          }
//        }";

//        string encryptedRequest = Encrypt(requestBody, key, iv);
//        Console.WriteLine("Encrypted Request Body (Hex):");
//        Console.WriteLine(encryptedRequest);
//    }

//    static string Encrypt(string plainText, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform encryptor = aes.CreateEncryptor())
//            {
//                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
//            }
//        }
//    }
//}


/// OTHER BANK TRANSFER
/// 

using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Key and IV
        string key = "2778f49ea5338e0f84535d37a78b63fc";
        string iv = "ebf582f617150b3e";

        // JSON Request Body
        string requestBody = @"{
          ""Request"": {
            ""Header"": {
              ""Timestamp"": ""20250120170624983"",
              ""ChannelDetails"": {
                ""ChannelID"": ""XPRESS"",
                ""ChannelType"": ""WEB"",
                ""ChannelSubClass"": """",
                ""BranchCode"": """",
                ""ChannelCusHdr"": {
                  ""ChannelProtocol"": """"
                }
              },
              ""DeviceDetails"": {
                ""DeviceID"": """",
                ""IMEINumber"": """",
                ""ClientIP"": """",
                ""OS"": """",
                ""BrowserType"": """",
                ""MobileNumber"": """",
                ""GeoLocation"": {
                  ""Latitude"": """",
                  ""Longitude"": """"
                }
              }
            },
            ""Body"": {
              ""SenderCD"": ""XPRESS_EX_WS"",
              ""TypeofPayment"": ""N"",
              ""TransactionID"": ""219517418521"",
              ""TransactionAmount"": ""125.00"",
              ""TransactionDate"": ""20250320"",
              ""SenderIFSC"": ""SIBL0000264"",
              ""SenderCustActType"": ""01"",
              ""RemitterAccNo"": ""0311199000005008"",
              ""RemitterAccName"": ""Xpress Money Services Ltd"",
              ""BeneficiaryIFSC"": ""UTIB0000119"",
              ""BeneficiaryCustActType"": ""40"",
              ""BeneficiaryAccNo"": ""623456000"",
              ""BeneficiaryAccName"": ""Nilesh Bhausaheb Katkar"",
              ""PurposeCD"": ""P1301"",
              ""CountryCD"": ""IN"",
              ""NotificationType"": ""SMS"",
              ""NotificationValue"": ""919999999999"",
              ""Signature"": """",
              ""CheckSum"": ""60-45-BD-14-8D-99"",
              ""RemitterDtl1"": ""SAMPLE ADDRESS"",
              ""RemitterDtl2"": ""UK"",
              ""RemitterDtl3"": ""UK"",
              ""RemitterDtl4"": ""UK"",
              ""BeneficiaryDtl1"": ""Kerala"",
              ""BeneficiaryDtl2"": ""Kerala"",
              ""BeneficiaryDtl3"": ""Kerala"",
              ""BeneficiaryDtl4"": ""Kerala"",
              ""RemitterToBeneficiaryInfo1"": ""NRE REMITTANCE"",
              ""RemitterToBeneficiaryInfo2"": ""NRE REMITTANCE"",
              ""RemitterToBeneficiaryInfo3"": ""NRE REMITTANCE"",
              ""RemitterToBeneficiaryInfo4"": ""NRE REMITTANCE"",
              ""UPI"": ""name@upiid.com""
            }
          }
        }";

        string encryptedRequest = Encrypt(requestBody, key, iv);
        Console.WriteLine("Encrypted Request Body (Hex):");
        Console.WriteLine(encryptedRequest);
    }

    static string Encrypt(string plainText, string key, string iv)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
            }
        }
    }
}


/// TRANSACTION STATUS INQUIRY
/// 

//using System;
//using System.Security.Cryptography;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "2778f49ea5338e0f84535d37a78b63fc";
//        string iv = "ebf582f617150b3e";

//        // JSON Request Body
//        string requestBody = @"{
//          ""Request"": {
//            ""Header"": {
//              ""Timestamp"": ""20250120173224983"",
//              ""ChannelDetails"": {
//                ""ChannelID"": ""XPRESS"",
//                ""ChannelType"": ""WEB"",
//                ""ChannelSubClass"": ""Retail"",
//                ""BranchCode"": """",
//                ""ChannelCusHdr"": {
//                  ""ChannelProtocol"": ""Mobile""
//                }
//              },
//              ""DeviceDetails"": {
//                ""DeviceID"": ""Device1"",
//                ""IMEINumber"": """",
//                ""ClientIP"": """",
//                ""OS"": """",
//                ""BrowserType"": """",
//                ""MobileNumber"": """",
//                ""GeoLocation"": {
//                  ""Latitude"": ""13.072090"",
//                  ""Longitude"": ""80.201859""
//                }
//              }
//            },
//            ""Body"": {
//              ""SenderCD"": ""XPRESS_EX_WS"",
//              ""TransactionID"": ""999218390509"",
//              ""TypeofPayment"": ""U"",
//              ""Signature"": """",
//              ""CheckSum"": ""60-45-BD-14-8D-99""
//            }
//          }
//        }";

//        string encryptedRequest = Encrypt(requestBody, key, iv);
//        Console.WriteLine("Encrypted Request Body (Hex):");
//        Console.WriteLine(encryptedRequest);
//    }

//    static string Encrypt(string plainText, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform encryptor = aes.CreateEncryptor())
//            {
//                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
//            }
//        }
//    }
//}


/// WALLET - VALIDATE UPI
///

//using System;
//using System.Security.Cryptography;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "2778f49ea5338e0f84535d37a78b63fc";
//        string iv = "ebf582f617150b3e";

//        // JSON Request Body
//        string requestBody = @"{
//          ""Request"": {
//            ""Header"": {
//              ""Timestamp"": ""20250120173224983"",
//              ""ChannelDetails"": {
//                ""ChannelID"": ""XPRESS"",
//                ""ChannelType"": ""WEB"",
//                ""ChannelSubClass"": ""Retail"",
//                ""BranchCode"": """",
//                ""ChannelCusHdr"": {
//                  ""ChannelProtocol"": ""Mobile""
//                }
//              },
//              ""DeviceDetails"": {
//                ""DeviceID"": ""Device1"",
//                ""IMEINumber"": """",
//                ""ClientIP"": """",
//                ""OS"": """",
//                ""BrowserType"": """",
//                ""MobileNumber"": """",
//                ""GeoLocation"": {
//                  ""Latitude"": ""13.072090"",
//                  ""Longitude"": ""80.201859""
//                }
//              }
//            },
//            ""Body"": {
//              ""SenderCD"": ""XPRESS_EX_WS"",
//              ""uniqueTranid"": ""1758112332"",
//              ""UPI_VPA_ID"": ""dinesh1@sib"",
//              ""Signature"": """",
//              ""CheckSum"": ""60-45-BD-14-8D-99""
//            }
//          }
//        }";

//        string encryptedRequest = Encrypt(requestBody, key, iv);
//        Console.WriteLine("Encrypted Request Body (Hex):");
//        Console.WriteLine(encryptedRequest);
//    }

//    static string Encrypt(string plainText, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform encryptor = aes.CreateEncryptor())
//            {
//                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
//            }
//        }
//    }
//}


/// WALLET - UPI FUND TRANSFER
///

//using System;
//using System.Security.Cryptography;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "2778f49ea5338e0f84535d37a78b63fc";
//        string iv = "ebf582f617150b3e";

//        // JSON Request Body
//        string requestBody = @"{
//          ""Request"": {
//            ""Header"": {
//              ""Timestamp"": ""20250120173224983"",
//              ""ChannelDetails"": {
//                ""ChannelID"": ""XPRESS"",
//                ""ChannelType"": ""WEB"",
//                ""ChannelSubClass"": ""Retail"",
//                ""BranchCode"": """",
//                ""ChannelCusHdr"": {
//                  ""ChannelProtocol"": ""Mobile""
//                }
//              },
//              ""DeviceDetails"": {
//                ""DeviceID"": ""Device1"",
//                ""IMEINumber"": """",
//                ""ClientIP"": """",
//                ""OS"": """",
//                ""BrowserType"": """",
//                ""MobileNumber"": """",
//                ""GeoLocation"": {
//                  ""Latitude"": ""13.072090"",
//                  ""Longitude"": ""80.201859""
//                }
//              }
//            },
//            ""Body"": {
//              ""sender_cd"": ""XPRESS_EX_WS"",
//              ""payment_type"": ""U"",
//              ""unique_tran_id"": ""91090115223"",
//              ""tran_amount"": ""100.00"",
//              ""tran_date"": ""20250220"",
//              ""relationship_acc_no"": ""0311199000005008"",
//              ""sender_details"": {
//                ""ifsc"": ""SIBL0000264"",
//                ""cust_act_type"": ""40""
//              },
//              ""remitter_details"": {
//                ""remitter_name"": ""Nilesh Katkar"",
//                ""remitter_country_code"": ""IN""
//              },
//              ""beneficiary_details"": {
//                ""ifsc"": """",
//                ""cust_act_type"": ""40"",
//                ""beneficiary_acc_no"": """",
//                ""beneficiary_acc_name"": ""Swarup Gadhve""
//              },
//              ""purpose_code"": ""P1301"",
//              ""notification_details"": {
//                ""type"": ""SMS"",
//                ""value"": ""918898329178""
//              },
//              ""Signature"": """",
//              ""checksum"": ""60-45-BD-14-8D-99"",
//              ""remitter_data"": {
//                ""remitterdtl1"": ""Pune"",
//                ""remitterdtl2"": ""Pune"",
//                ""remitterdtl3"": """",
//                ""remitterdtl4"": """"
//              },
//              ""beneficiary_data"": {
//                ""beneficiarydtl1"": ""India"",
//                ""beneficiarydtl2"": ""India"",
//                ""beneficiarydtl3"": """",
//                ""beneficiarydtl4"": """"
//              },
//              ""remitter_beneficiary_info"": {
//                ""info_details1"": ""Travel"",
//                ""info_details2"": ""and Info"",
//                ""info_details3"": """",
//                ""info_details4"": """"
//              },
//              ""UPI"": ""dinesh@sib""
//    }
//          }
//        }";

//        string encryptedRequest = Encrypt(requestBody, key, iv);
//        Console.WriteLine("Encrypted Request Body (Hex):");
//        Console.WriteLine(encryptedRequest);
//    }

//    static string Encrypt(string plainText, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform encryptor = aes.CreateEncryptor())
//            {
//                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//                return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
//            }
//        }
//    }
//}