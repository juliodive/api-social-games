﻿using SocialGames.Domain.Arguments.PlatForm;
using SocialGames.Domain.Entities;
using SocialGames.Domain.Interfaces.Repositories;
using SocialGames.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SocialGames.Domain.Services
{
    public class ServicePlatForm : IServicePlatForm
    {
        private readonly IRepositoryPlatForm _repositoryPlatForm;
        private readonly IRepositoryGame _repositoryGame;

        public ServicePlatForm(IRepositoryPlatForm repositoryPlatForm, IRepositoryGame repositoryGame)
        {
            _repositoryPlatForm = repositoryPlatForm;
            _repositoryGame = repositoryGame;
        }

        public PlatFormResponse Create(CreatePlatFormRequest request)
        {
            var platForm = new PlatForm(request.Name);
            if (_repositoryPlatForm.Exists(
                x => x.Name.ToString().ToLower().Replace(" ", "") ==
                request.Name.ToString().ToLower().Replace(" ", "")))
            {
                throw new ValidationException("This PlatForm already exists!");
            }
            var result = _repositoryPlatForm.Create(platForm);

            return (PlatFormResponse)result;

        }

        public IEnumerable<PlatFormResponse> GetAll()
        {
            return _repositoryPlatForm.List().ToList().Select(x => (PlatFormResponse)x).ToList();
        }

        public PlatFormResponse GetById(Guid id)
        {
            var platForm = ExistPlatForm(id);

            return (PlatFormResponse)platForm;
        }

        public PlatFormResponse Update(Guid id, UpdatePlatFormRequest request)
        {
            var platForm = ExistPlatForm(id);

            platForm.ChancePlatForm(request.Name);
            var result = _repositoryPlatForm.Update(platForm);

            return (PlatFormResponse)result;
        }
        public void Delete(Guid id)
        {
            var platForm = ExistPlatForm(id);
            var game = _repositoryGame.List().Any(x => x.PlatFormId == id);
            if (game == true) throw new ValidationException("This Platform Id is linked to Game!");
            _repositoryPlatForm.Delete(platForm);
        }

        private PlatForm ExistPlatForm(Guid id)
        {
            var platForm = _repositoryPlatForm.GetById(id);

            if (platForm == null) throw new ValidationException("Id PlatForm not Found");

            return platForm;
        }
    }
}
