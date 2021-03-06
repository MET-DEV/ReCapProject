﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOS;
using FluentValidation;
using MyCore.Aspects.Autofac.Validation;
using MyCore.CrossCuttingConcerns.Validation;
using MyCore.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }
        [SecuredOperation("car.add,admin")]
        [ValidationAspect(typeof(CarValidator))]
        public IResult Add(Car car)
        {

            _carDal.Add(car);
            return new SuccessDataResult(Messages.CarAdded);
        }

        public IDataResult< List<Car>> GetAll()
        {
            if (DateTime.Now.Hour==20)
            {
                return new ErrorDataResult<List<Car>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(),Messages.CarListed);
        }

        public IDataResult<List<Car>>  GetByBrandId(int brandId)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.BrandId == brandId));
        }

        public IDataResult<Car>  GetById(int carId)
        {
            return new SuccessDataResult<Car>(_carDal.Get(c=>c.Id==carId));
        }

        public IDataResult<List<Car>>  GetByPrice(decimal min,decimal max)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.DailyPrice <= max && c.DailyPrice >= min));
        }

        

        public IDataResult<List<CarDetailDto>>  GetCarDetails()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<CarDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(),message: Messages.CarListed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsBrandId(int brandId)
        {
            var context = _carDal.GetCarDetails().Where(r => r.BrandId == brandId).ToList();
            return new SuccessDataResult<List<CarDetailDto>>(context);
        }

        public IDataResult<CarDetailDto> GetCarDetailsCarId(int carId)
        {

            var context = _carDal.GetCarDetails().SingleOrDefault(c => c.CarId == carId);
            return new SuccessDataResult<CarDetailDto>(context);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsColorId(int colorId)
        {
            var context = _carDal.GetCarDetails().Where(r => r.ColorId == colorId).ToList();
            return new SuccessDataResult<List<CarDetailDto>>(context);
        }
    }
}
