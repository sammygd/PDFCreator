﻿using NUnit.Framework;
using pdfforge.PDFCreator.UI.Interactions;
using pdfforge.PDFCreator.UI.Interactions.Enums;
using pdfforge.PDFCreator.UI.ViewModels.DialogViewModels;
using pdfforge.PDFCreator.UI.ViewModels.DialogViewModels.Translations;
using pdfforge.PDFCreator.UnitTest.UnitTestHelper;

namespace pdfforge.PDFCreator.UnitTest.UI.ViewModels.DialogViewModels
{
    [TestFixture]
    internal class EncryptionPasswordViewModelTest
    {
        [Test]
        public void OkCommand_CanExecute_IfInteractionIsNotSet_ReturnsFalse()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            Assert.IsFalse(viewModel.OkCommand.CanExecute(null));
        }

        [Test]
        public void OkCommand_CanExecute_IfSkipNotShown_IsTrue()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            viewModel.SetInteraction(new EncryptionPasswordInteraction(false, true, true));
            Assert.IsTrue(viewModel.OkCommand.CanExecute(null));
        }

        [Test]
        public void OkCommand_CanExecute_OwnerPasswordIsRequiredAndNotSet_IsFalse()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            viewModel.SetInteraction(new EncryptionPasswordInteraction(true, true, false));
            Assert.IsFalse(viewModel.OkCommand.CanExecute(null));
        }

        [Test]
        public void OkCommand_CanExecute_RequiredPasswordsAreSet_IsTrue()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            viewModel.SetInteraction(new EncryptionPasswordInteraction(true, false, true));
            viewModel.Interaction.UserPassword = "MyPassword";
            Assert.True(viewModel.OkCommand.CanExecute(null));
        }

        [Test]
        public void OkCommand_CanExecute_UserPasswordIsRequiredAndNotSet_IsFalse()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            viewModel.SetInteraction(new EncryptionPasswordInteraction(true, false, true));
            Assert.IsFalse(viewModel.OkCommand.CanExecute(null));
        }

        [Test]
        public void OkCommand_OnExecute_CompletesInteraction()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            var interaction = new EncryptionPasswordInteraction(false, true, true);

            var helper = new InteractionHelper<EncryptionPasswordInteraction>(viewModel, interaction);

            viewModel.OkCommand.Execute(null);

            Assert.AreEqual(PasswordResult.StorePassword, interaction.Response);
            Assert.IsTrue(helper.InteractionIsFinished);
        }

        [Test]
        public void OnInteractionSet_SetsPasswordsInView()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            var interaction = new EncryptionPasswordInteraction(false, true, true);

            var actionWasCalled = false;
            viewModel.SetPasswordInUi = (x, y) => actionWasCalled = true;

            viewModel.SetInteraction(interaction);
            Assert.IsTrue(actionWasCalled);
        }

        [Test]
        public void RemoveCommand_OnExecute_CompletesInteraction()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            var interaction = new EncryptionPasswordInteraction(false, true, true);
            var helper = new InteractionHelper<EncryptionPasswordInteraction>(viewModel, interaction);
            viewModel.RemoveCommand.Execute(null);

            Assert.AreEqual(PasswordResult.RemovePassword, interaction.Response);
            Assert.IsTrue(helper.InteractionIsFinished);
            Assert.AreEqual("", interaction.OwnerPassword);
            Assert.AreEqual("", interaction.UserPassword);
        }

        [Test]
        public void SetingOwnerPassword_WritesToInteractionAndCallsCanExecuteChanged()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            var canExecuteChanged = false;
            viewModel.OkCommand.CanExecuteChanged += (sender, args) => canExecuteChanged = true;
            var interaction = new EncryptionPasswordInteraction(false, true, true);
            viewModel.SetInteraction(interaction);

            viewModel.OwnerPassword = "MyPassword";

            Assert.AreEqual("MyPassword", interaction.OwnerPassword);
            Assert.IsTrue(canExecuteChanged);
        }

        [Test]
        public void SetingUserPassword_WritesToInteractionAndCallsCanExecuteChanged()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            var canExecuteChanged = false;
            viewModel.OkCommand.CanExecuteChanged += (sender, args) => canExecuteChanged = true;
            var interaction = new EncryptionPasswordInteraction(false, true, true);
            viewModel.SetInteraction(interaction);

            viewModel.UserPassword = "MyPassword";

            Assert.AreEqual("MyPassword", interaction.UserPassword);
            Assert.IsTrue(canExecuteChanged);
        }

        [Test]
        public void SkipCommand_OnExecute_CompletesInteraction()
        {
            var viewModel = new EncryptionPasswordViewModel(new EncryptionPasswordsWindowTranslation());
            var interaction = new EncryptionPasswordInteraction(false, true, true);
            var helper = new InteractionHelper<EncryptionPasswordInteraction>(viewModel, interaction);
            viewModel.SkipCommand.Execute(null);

            Assert.AreEqual(PasswordResult.Skip, interaction.Response);
            Assert.IsTrue(helper.InteractionIsFinished);
            Assert.AreEqual("", interaction.OwnerPassword);
            Assert.AreEqual("", interaction.UserPassword);
        }
    }
}