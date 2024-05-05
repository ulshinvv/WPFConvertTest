using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValuteConverter;

namespace ViewModelTestss
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        public void CoinYepParser_GetValuteEntries_Should_Return_Correct_List()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Vname");
            dataTable.Columns.Add("Vchcode");
            dataTable.Columns.Add("Vcurs");
            dataTable.Rows.Add("Доллар США", "USD", 91.69);

            // Act
            var valuteEntries = CoinYepParser.GetValuteEntries(dataTable);

            // Assert
            Assert.IsNotNull(valuteEntries);
            Assert.AreEqual(2, valuteEntries.Count()); // Должно быть 2 записи: RUB и USD
        }

        [Test]
        public void InputHandler_Unify_Should_Return_Correct_Unified_String()
        {
            // Arrange
            string input = "1000.50";

            // Act
            var unifiedInput = InputHandler.Unify(input);

            // Assert
            Assert.AreEqual("1000,50", unifiedInput);
        }
    }
}
