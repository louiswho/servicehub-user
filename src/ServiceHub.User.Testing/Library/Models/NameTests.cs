using System;
using Xunit;
using ServiceHub.User.Library.Models;

namespace ServiceHub.User.Testing.Library.Models
{
    public class NameTests
    {
        /// <value> A string over the max limit size for Name. </value>
        readonly string OversizedName = new string('A', 256);
        /// <value> A string of max size for Name. </value>
        readonly string MaxLengthName = new string('A', 255);
        /// <value> An empty string. </value>
        readonly string UndersizedName = new string('A', 0);
        /// <value> A string of minimum size for Name. </value>
        readonly string SingleCharacterName = new string('A', 1);

        /// <value> A generic name that will serve as the baseline for the tests. </value>
        readonly Name ControlName = new Name() { NameId = Guid.NewGuid(), First = "John", Middle = "Jacob", Last = "Schmidt" };

        #region Validate
        /// <summary>
        /// Test complete name for compliance with model requirements.
        /// </summary>
        [Fact]
        [Trait("Type", "ControlGroup")]
        public void Validate_CompleteName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to check that an unset guid is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void Validate_GuidIsNotSet_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.NameId = Guid.Empty;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure a missing first name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void Validate_FirstIsNull_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.First = null;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure a missing first name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "NotRequiredField")]
        public void Validate_MiddleIsNull_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.Middle = null;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to make sure a missing Last name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void Validate_LastIsNull_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.Last = null;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure an empty first name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "EmptyString")]
        public void Validate_EmptyFirstName_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.First = UndersizedName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure an empty middle name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "EmptyString")]
        public void Validate_EmptyMiddleName_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.Middle = UndersizedName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure an empty last name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "EmptyString")]
        public void Validate_EmptyLastName_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.Last = UndersizedName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure a single character first name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "EmptyString")]
        public void Validate_SingleCharacterFirstName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.First = SingleCharacterName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to make sure a single character middle name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "EmptyString")]
        public void Validate_SingleCharacterMiddleName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.Middle = SingleCharacterName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to make sure a single character last name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "EmptyString")]
        public void Validate_SingleCharacterLastName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.Last = SingleCharacterName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to make sure a 256-character first name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void Validate_OversizedFirstName_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.First = OversizedName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure a 256-character middle name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void Validate_OversizedMiddleName_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.Middle = OversizedName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure a 256-character last name is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void Validate_OversizedLastName_ReturnsFalse()
        {
            //Arrange
            Name name = ControlName;
            name.Last = OversizedName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test to make sure a 255-character first name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void Validate_MaxLengthFirstName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.First = MaxLengthName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to make sure a 255-character middle name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void Validate_MaxLengthMiddleName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.Middle = MaxLengthName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to make sure a 255-character last name is valid.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void Validate_MaxLengthLastName_ReturnsTrue()
        {
            //Arrange
            Name name = ControlName;
            name.Last = MaxLengthName;

            //Act
            var result = name.Validate();

            //Assert
            Assert.True(result);
        }

        #endregion

        #region ValidateFirst
        /// <summary>
        /// Tests to make sure a null first name would be invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void ValidateFirst_NullArgument_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(null);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an empty string would not be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void ValidateFirst_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(UndersizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an oversized string would not be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void ValidateFirst_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(OversizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure a 255 letter string would be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateFirst_MaxLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(MaxLengthName);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure a one letter string would be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateFirst_SingleCharacterName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(SingleCharacterName);

            // Assert
            Assert.True(result);
        }
        #endregion

        #region ValidateMiddle
        /// <summary>
        /// Tests to make sure a null middle name would be valid.
        /// </summary>
        [Fact]
        [Trait("Type", "NotRequiredField")]
        public void ValidateMiddle_NullArgument_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(null);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure an empty string would not be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void ValidateMiddle_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(UndersizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an oversized string would not be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void ValidateMiddle_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(OversizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure a 255 letter string would be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateMiddle_MaxLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(MaxLengthName);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure a one letter string would be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateMiddle_SingleCharacterName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(SingleCharacterName);

            // Assert
            Assert.True(result);
        }
        #endregion

        #region ValidateLast
        /// <summary>
        /// Tests to make sure a null last name would be invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void ValidateLast_NullArgument_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(null);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an empty string would not be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void ValidateLast_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(UndersizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an oversized string would not be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "MaxStringLength")]
        public void ValidateLast_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(OversizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure a 255 letter string would be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateLast_MaxLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(MaxLengthName);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure a one letter string would be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateLast_SingleCharacterName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(SingleCharacterName);

            // Assert
            Assert.True(result);
        }
        #endregion
    }
}
