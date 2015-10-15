/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots.Extensions
{
    internal class NonOverlappingYAxis
    {
        public NonOverlappingYAxis()
        {
            _maxRetries = 100;
            _decrementUnit = 0.1;
            _intervals = new Dictionary<uint, SortedList<Interval, double>>();
        }

        private Dictionary<UInt32, SortedList<Interval, double>> _intervals { set; get; }
        private double _yCoordinate { set; get; }
        private double _decrementUnit { set; get; }
        int _maxRetries { set; get; }

        public void Reset()
        {
            _intervals.Clear();
        }
        public double Coordinate(UInt32 sampleID, int left, int right, int baseCoordinate)
        {
            if (!_intervals.ContainsKey(sampleID))
                _intervals.Add(sampleID, new SortedList<Interval, double>());

            var newInterval = new Interval(left, right);
            var overlappingIntervals = _intervals[sampleID].Where(x => x.Key.Overlap(newInterval) == true).ToList();

            if (overlappingIntervals.Count == 0)
            {
                newInterval.yCoordinate = baseCoordinate;
                _intervals[sampleID].Add(newInterval, baseCoordinate);
                return baseCoordinate;
            }

            int coefficient = 0;
            int tries = 0;
            double decrementUnit = _decrementUnit;
            while (tries < _maxRetries)
            {
                tries++;
                _yCoordinate = Math.Round(baseCoordinate - (coefficient++ * decrementUnit), 4);
                if ((overlappingIntervals.FirstOrDefault(x => x.Value == _yCoordinate)).Key == null)
                {
                    newInterval.yCoordinate = _yCoordinate;
                    _intervals[sampleID].Add(newInterval, _yCoordinate);
                    return _yCoordinate;
                }

                if (Math.Abs(baseCoordinate) != Math.Floor(Math.Abs(_yCoordinate - decrementUnit)))
                {
                    decrementUnit = decrementUnit / Math.Pow(10, (decrementUnit + "").Split('.')[1].Length);
                    _yCoordinate = baseCoordinate;
                    coefficient = 0;
                }
            }
            return baseCoordinate;
        }

        private class Interval : IComparable<Interval>
        {
            public Interval(int left, int right)
            {
                this.left = left;
                this.right = right;
            }
            public int left { private set; get; }
            public int right { private set; get; }
            public double yCoordinate { set; get; }
            public override bool Equals(object obj)
            {
                Interval other = (Interval)obj;
                int comparison = this.right.CompareTo(other.right);
                if (comparison != 0) return false;
                comparison = this.left.CompareTo(other.left);
                if (comparison != 0) return false;
                comparison = this.yCoordinate.CompareTo(other.yCoordinate);
                if (comparison != 0) return false;
                return true;
            }
            public int CompareTo(Interval other)
            {
                int comparison = this.right.CompareTo(other.right);
                if (comparison != 0) return comparison;
                comparison = this.left.CompareTo(other.left);
                if (comparison != 0) return comparison;
                return this.yCoordinate.CompareTo(other.yCoordinate);
            }
            public bool Overlap(Interval other)
            {
                if (this.right < other.left) return false;
                if (this.left > other.right) return false;
                else return true;
            }
        }
    }
}
