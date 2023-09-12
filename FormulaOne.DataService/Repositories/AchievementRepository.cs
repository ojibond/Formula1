﻿using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaOne.DataService.Repositories
{
    public class AchievementRepository : GenericRepository<Achievment>, IAchievementRepository
    {
        public AchievementRepository(AppDbContext context, ILogger logger) : base(context, logger)
        { }

        public async Task<Achievment?> GetDriverAchievmentAsync(Guid driverId)
        {
            try
            {
               return await _dbSet.FirstOrDefaultAsync(x => x.Id == driverId);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} GetDriverAchievmentAsync function error", typeof(AchievementRepository));
                throw;
            }
        }

        public override async Task<IEnumerable<Achievment>> All()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderBy(x => x.AddedDate)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(AchievementRepository));
                throw;
            }
        }

        public override async Task<bool> Delete(Guid id)
        {
            try
            {
                //Get my entity
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

                if (result == null)
                    return false;

                result.Status = 0;
                result.UpdateDate = DateTime.UtcNow;

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} Delete function error", typeof(AchievementRepository));
                throw;
            }
        }

        public override async Task<bool> Update(Achievment achievement)
        {
            try
            {
                //Get my entity
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == achievement.Id);

                if (result == null)
                    return false;

                result.UpdateDate = DateTime.UtcNow;
                result.FastestLap = achievement.FastestLap;
                result.PolePosition = achievement.PolePosition;
                result.RaceWins = achievement.RaceWins;
                result.WorldChampionship = achievement.WorldChampionship;

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} Update function error", typeof(AchievementRepository));
                throw;
            }
        }
    }
}
