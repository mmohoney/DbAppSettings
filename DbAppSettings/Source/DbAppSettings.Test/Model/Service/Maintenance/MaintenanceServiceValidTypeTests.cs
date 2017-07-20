using System;
using System.Collections.Specialized;
using DbAppSettings.Model.Service.Maintenance;
using NUnit.Framework;

namespace DbAppSettings.Test.Model.Service.Maintenance
{
    [TestFixture]
    public class MaintenanceServiceValidTypeTests
    {
        [Test]
        public void bool_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("true", typeof(bool).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void bool_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("not_valid_value", typeof(bool).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void byte_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("255", typeof(byte).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void byte_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("256", typeof(byte).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void char_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("a", typeof(char).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void char_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("aa", typeof(char).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void decimal_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("1.1", typeof(decimal).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void decimal_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("a", typeof(decimal).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void double_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("1.1", typeof(double).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void double_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("a", typeof(double).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void float_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("1.1", typeof(float).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void float_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("a", typeof(float).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void int_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("1", typeof(int).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void int_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("a", typeof(int).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void long_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("9223372036854775807", typeof(long).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void long_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("9223372036854775808", typeof(long).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void sbyte_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("-127", typeof(sbyte).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void sbyte_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("128", typeof(sbyte).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void short_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("256", typeof(short).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void short_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("32769", typeof(short).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void string_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("test string", typeof(string).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void string_invalid()
        {
            //Probably not the greatest test since invalid strings are hard to come by...
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType(new object(), typeof(string).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void StringCollection_valid()
        {
            var exampleString = @"Item 1
                                    Item 2
                                    Item 3
                                    ";
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType(exampleString, typeof(StringCollection).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void StringCollection_invalid()
        {
            //Probably not the greatest test since invalid strings are hard to come by...
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType(new object(), typeof(StringCollection).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void DateTime_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("12/21/1978 14:00", typeof(DateTime).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void DateTime_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("13/21/1978", typeof(DateTime).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void Guid_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("c431546c-7587-4c03-836a-c336b0199ef2", typeof(Guid).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void Guid_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("c431546c-7587-4c03-836a-c336b0199ef21", typeof(Guid).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void TimeSpan_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("00:00:01", typeof(TimeSpan).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void TimeSpan_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("123456789", typeof(TimeSpan).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void uint_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("4294967295", typeof(uint).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void uint_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("4294967296", typeof(uint).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void ulong_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("18446744073709551615", typeof(ulong).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void ulong_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("18446744073709551616", typeof(ulong).FullName);
            Assert.IsFalse(result);
        }

        [Test]
        public void ushort_valid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("65535", typeof(ushort).FullName);
            Assert.IsTrue(result);
        }

        [Test]
        public void ushort_invalid()
        {
            var result = new DbAppSettingMaintenanceService(null).ValidateValueForType("65536", typeof(ushort).FullName);
            Assert.IsFalse(result);
        }
    }
}
