using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ukol_v1;
using Xunit;

namespace Ukol_v1_tests
{

    public class MainViewModelTests
    {
        [Fact]
        public void LoadXmlFile_ValidFile_AddsSummaries()
        {
            // Arrange
            var mockFileDialog = new Mock<IFileDialogService>();
            var mockMessageBox = new Mock<IMessageBoxService>();

            string testXml = """
        <?xml version="1.0" encoding="utf-8"?>
        <Automobily>
          <Auto>
            <NazevModelu>Škoda Oktávia</NazevModelu>
            <DatumProdeje>2010-12-02</DatumProdeje>
            <Cena>500.000</Cena>
            <DPH>20</DPH>
          </Auto>
        </Automobily>
        
        """;

            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, testXml);

            mockFileDialog.Setup(f => f.OpenFile(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(tempFile);

            var viewModel = new MainViewModel(mockFileDialog.Object, mockMessageBox.Object);

            viewModel.LoadXmlFile();

            Assert.Single(viewModel.AutoSummaries);
            mockMessageBox.Verify(m => m.Show(It.IsAny<string>()), Times.Never);

            File.Delete(tempFile);
        }
    }
}