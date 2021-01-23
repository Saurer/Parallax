// Parallax
// Copyright (C) 2020  Frank Horrigan <https://github.com/saurer>

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.ComponentModel.DataAnnotations;
using AuroraCore;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class AddModelRelationData {
        private int cardinality = 1;
        private bool mutable = Const.DefaultMutability;

        [Required]
        public IRelation Relation { get; set; }
        public bool Required { get; set; } = Const.DefaultRequired;
        public bool Mutable {
            get {
                return mutable;
            }
            set {
                if (value) {
                    cardinality = 1;
                }

                mutable = value;
            }
        }

        [Range(0, Int32.MaxValue)]
        public int Cardinality {
            get {
                return Mutable ? 1 : cardinality;
            }
            set {
                cardinality = value;
            }
        }

        public int? Permission { get; set; }

        public bool Valid =>
            Cardinality >= 0;

        public ConditionData Conditions { get; set; } = new ConditionData();
    }
}