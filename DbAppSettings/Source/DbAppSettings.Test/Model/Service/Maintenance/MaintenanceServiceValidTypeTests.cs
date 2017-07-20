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
    }
}
