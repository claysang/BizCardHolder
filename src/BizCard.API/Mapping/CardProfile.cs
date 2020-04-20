using AutoMapper;
using BizCard.API.ViewModel;
using BizCard.Core.Models;

namespace BizCard.API.Mapping
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardViewModel>();
            CreateMap<CardViewModel, Card>();
        }
    }
}
