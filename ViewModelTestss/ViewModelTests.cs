using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValuteConverter.Model.Interfaces;
using ValuteConverter;
using Moq;

namespace ViewModelTestss
{
    [TestFixture]
    public class ViewModelTests
    {
        [Test]
        public async Task LoadValutes_Should_Load_ValuteEntries_From_CoinYepParser()
        {
            // Arrange
            var mockConverter = new Mock<ICourseConverter>();
            var viewModel = new ViewModel(mockConverter.Object);

            var dataTable = new DataTable();
            dataTable.Columns.Add("Vname");
            dataTable.Columns.Add("Vchcode");
            dataTable.Columns.Add("Vcurs");
            dataTable.Rows.Add("Доллар США", "USD", 91.69);

            mockConverter.Setup(c => c.GetExchangeRateOnDateAsync(It.IsAny<DateTime>())).ReturnsAsync(dataTable);

            // Act
            await viewModel.LoadValutes();

            // Assert
            Assert.IsNotNull(viewModel.ValuteEntries);
            Assert.AreEqual(2, viewModel.ValuteEntries.Count);
        }

        [Test]
        public void ConvertLeftToRight_Should_Convert_Left_To_Right_Text()
        {
            // Arrange
            var mockConverter = new Mock<ICourseConverter>();
            var viewModel = new ViewModel(mockConverter.Object);

            var rubValute = new ValuteEntry("Российский рубль", "RUB", 1.0);
            var usdValute = new ValuteEntry("Доллар США", "USD", 91.69);
            viewModel.SelectedValuteLeft = rubValute;
            viewModel.SelectedValuteRight = usdValute;
            viewModel.LeftText = "9169";

            // Act
            viewModel.ConvertLeftToRight();

            // Assert
            Assert.That(viewModel.RightText, Is.EqualTo("100")); 
        }
    }
}
