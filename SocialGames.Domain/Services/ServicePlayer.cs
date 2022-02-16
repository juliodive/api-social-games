﻿using SocialGames.Domain.Arguments.Base;
using SocialGames.Domain.Arguments.Player;
using SocialGames.Domain.Entities;
using SocialGames.Domain.Extensions;
using SocialGames.Domain.Interfaces.Repositories;
using SocialGames.Domain.Interfaces.Services;
using SocialGames.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SocialGames.Domain.Services
{
    public class ServicePlayer : IServicePlayer
    {
        private readonly IRepositoryPlayer _repositoryPlayer;

        public ServicePlayer(IRepositoryPlayer repositoryPlayer)
        {
            _repositoryPlayer = repositoryPlayer;
        }

        public AuthenticatePlayerResponse Authenticate(AuthenticatePlayerRequest request)
        {
            if (request == null)
            {
                throw new ValidationException("AuthenticatePlayerRequest is required!");
            }

            var player = _repositoryPlayer.GetBy(
                x => x.Email.Address == request.Email &&
                x.Password.Word == request.Password.ConvertToMD5());
            return (AuthenticatePlayerResponse)player;
        }

        public CreatePlayerResponse Create(CreatePlayerRequest request)
        {
            var name = new Name(request.FirstName, request.LastName);
            var email = new Email(request.Email.ToString().Replace("%40","@"));
            var password = new Password(request.Password);

            Player player = new Player(name, email, password);
            if (_repositoryPlayer.Exists(x => x.Email.Address == request.Email))
            {
                throw new ValidationException("This User already exists!");
            }
            player = _repositoryPlayer.Create(player);
            return (CreatePlayerResponse)player;
        }
        
        public IEnumerable<PlayerResponse> GetAllPlayers()
        {
            return _repositoryPlayer.List().ToList().Select(x => (PlayerResponse)x).ToList();

        }

        public PlayerResponse GetById(Guid id)
        {
            var player = _repositoryPlayer.GetById(id);
            if (player == null)
            {
                throw new ValidationException("Player not found!");
            }
            return (PlayerResponse)player;
        }

        public UpdateAdminPlayerResponse UpdateAdmin(UpdateAdminPlayerRequest request)
        {
            if (request == null)
            {
                throw new ValidationException("ChancePlayerRequest is required!");
            }

            Player player = _repositoryPlayer.GetById(request.Id);
            if (player == null)
            {
                throw new ValidationException("Player not found!");
            }
            player.UpdatePlayerAdmin(player.Status);
            _repositoryPlayer.Update(player);

            return (UpdateAdminPlayerResponse)player;
        }
        public UpdatePlayerResponse Update(UpdatePlayerRequest request)
        {
            if (request == null)
            {
                throw new ValidationException("ChancePlayerRequest is required!");
            }

            Player player = _repositoryPlayer.GetById(request.Id);
            if (player == null)
            {
                throw new ValidationException("Player not found!");
            }
            var email = new Email(request.Email);
            var name = new Name(request.FirstName, request.LastName);

            player.UpdatePlayer(name, email, player.Status);
            _repositoryPlayer.Update(player);

            return (UpdatePlayerResponse)player;
        }
        public ResponseBase Delete(Guid id)
        {
            Player player = _repositoryPlayer.GetById(id);
            if (player == null)
            {
                throw new ValidationException("Player not found!");
            }
            _repositoryPlayer.Delete(player);  

            return (ResponseBase)player;
        }
    }
}
