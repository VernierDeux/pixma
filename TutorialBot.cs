using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pirates;
namespace MyBot
{
    public class TutorialBot : Pirates.IPirateBot
    {
        public int abs(int num)
        {
            if (num < 0)
            {
                num = num * -1;
            }
            return num;
        }
        public void areweready(PirateGame game)
        {
            Location a;
            List<Drone> motek = new List<Drone>();
            foreach (Drone d in game.GetMyLivingDrones())
            {
                if (howmuchdronesonthesameplace(game, d) != null)
                {
                    a = howmuchdronesonthesameplace(game, d);
                    foreach (Drone c in game.GetMyLivingDrones())
                    {
                        if (c.Location == a)
                            motek.Add(c);
                    }
                }
            }
            normaldrones(game,motek);

        }
        //public void radarattack2(Pirate a, PirateGame game)
        //{
        //    foreach (Aircraft b in game.GetEnemyLivingAircrafts())
        //    {
        //        if (b.Location.Col >= 0 && b.Location.Col <= 17 && b.Location.Row > 24 && b.Location.Row <= 31)
        //        {
        //            if (doyouhaveatarget(a, game) != null)
        //            {
        //                attack(game, a, doyouhaveatarget(a, game));
        //            }
        //        }
        //    }
        //}
        //public void radarattack(Pirate a, PirateGame game)
        // {
        // foreach (Aircraft b in game.GetEnemyLivingAircrafts())
        //  {
        //   if (b.Location.Col >= 0 && b.Location.Col <= 18 && b.Location.Row >= 18 && b.Location.Row <= 24)
        //    {
        //        if (doyouhaveatarget(a, game) != null)
        //      {
        //         attack(game, a, doyouhaveatarget(a, game));
        //     }
        // }
        //}
        //   }
        public Location howmuchdronesonthesameplace(PirateGame game, Drone a)
        {
            int i = 0;
            foreach (Drone d in game.GetMyLivingDrones())
            {
                if (a.Distance(d) <= 1)
                {
                    i++;
                }

            }
            if (i >= 5)
                return a.Location;
            return null;
        }
        public bool emergency(PirateGame game)
        {
            foreach (Island a in game.GetAllIslands())
            {
                if (a.Owner != game.GetEnemy())
                {
                    return false;
                }
            }
            return true;
        }
        public int rateisland(PirateGame game, Island a, Pirate p)
        {
            int points = 0;
            points += 5 - howmuchenemypiratesinthisisland(game, a); // if there are 0 pirates - it will give 5 points!
            points += 6 - givepointsbydistance(game, p, a); // if this is the closest island - it will give 5 points!
            return points;

        }
        public void normaldrones(PirateGame game,List<Drone> a)
        {
            Location city = game.GetMyCities()[0].Location;
            foreach (Drone drone in a)
            {
                game.SetSail(drone, game.GetSailOptions(drone, city)[getRandDroneWay(drone.Id,game.GetSailOptions(drone,city).Count)]);
            }
        }
        public bool emergency1(PirateGame game)
        {
            foreach (Island a in game.GetAllIslands())
            {
                if (a.Owner == game.GetEnemy())
                {
                    return true;
                }
            }
            return false;
        }
        public bool goodtous(PirateGame game)
        {
            int count = 0;
            foreach (Island a in game.GetAllIslands())
            {
                if (a.Owner == game.GetMyself())
                {
                    count++;
                }
            }
            return count >= 2;
        }
        public int getRandDroneWay(int id, int max){
            if(id<max)
             return id;
            return id%max;
        }
        public bool imashazona(int y, int x, int cityY, int cityX,PirateGame game){
            int mapX = game.GetColCount();
            int mapY = game.GetRowCount();
            game.Debug("ok1");
            game.Debug(cityX - x);
            if(cityX - x < 0){
                // Right
                game.Debug("ok2");
                if(abs(cityY - y) <= 5){
                    game.Debug("Ata zodek");
                    return true;
                }
            }
            if(x - cityX < 0){
                // Left
                if(abs(cityY - y) <= 5){
                    game.Debug("Ata zodek");
                    return true;
                }
            }
            return false;
        }
        public void handleDrones(List<Drone> d, PirateGame game)
        {
            int mapX = game.GetColCount() - 1;
            int mapY = game.GetRowCount() - 1;
            City myCity = game.GetMyCities()[0];
            Location myLoc = myCity.Location;
            int cityX = myLoc.Col;  // X
            int cityY = myLoc.Row; // Y
            bool rahok = false;
            foreach (Drone dro in d)
            {
                if (dro.Location.Col != 0 && dro.Location.Row != 0 && dro.Location.Col != mapX && dro.Location.Row != cityY && !imashazona(dro.Location.Row,dro.Location.Col,cityY,cityX,game))
                {
                    game.Debug("YES LEVEL 1");
                    // LEVEL 1  
                    if (abs(cityX - mapX) < cityX)
                    {
                        // Rahok
                        Location loc = new Location(dro.Location.Row, mapX);
                        //game.Debug(loc);
                        game.SetSail(dro, game.GetSailOptions(dro, loc)[0]);
                    }
                    else
                    {
                        //Karov
                        Location loc = new Location(dro.Location.Col, 0);
                        //game.Debug(loc);
                        game.SetSail(dro, game.GetSailOptions(dro, loc)[0]);
                    }
                }
                else if ((dro.Location.Row == cityY - 5 || dro.Location.Row == cityY + 5) || imashazona(dro.Location.Row,dro.Location.Col,cityY,cityX,game)){
                    // Split drones
                        List<Location> m = game.GetSailOptions(dro,new Location(cityY,cityX));
                        game.SetSail(dro,m[getRandDroneWay(dro.Id,m.Count)]);
                }
                
                else if (dro.Location.Row == cityY)
                {
                    // LEVEL 3 OVERRIDE
                    game.Debug("BLA BLA");
                    //game.Debug(game.GetSailOptions(dro,new Location(cityY,dro.Location.Col))[0]);
                    game.SetSail(dro, game.GetSailOptions(dro, new Location(dro.Location.Row, cityX))[0]);
                }
                else if ((dro.Location.Col == mapX || dro.Location.Col == 0) && dro.Location.Row != cityY && !imashazona(dro.Location.Row,dro.Location.Col,cityY,cityX,game) )
                {
                    // LEVEL 2
                     game.Debug("ahaln ma nishma");
                    Location loc = new Location(cityY, dro.Location.Col);
                    game.SetSail(dro, game.GetSailOptions(dro, loc)[0]);
                }
                else
                {
                    // LEVEL 3
                     game.Debug("HEY");
                    //game.Debug(game.GetSailOptions(dro,new Location(cityY,dro.Location.Col))[0]);
                    game.SetSail(dro, game.GetSailOptions(dro, new Location(dro.Location.Row, cityX))[0]);
                }

            }

        }
        public bool isSomeoneThere(Location loc,PirateGame game){
            List<Aircraft> bobo = game.GetMyLivingAircrafts();
            bool momo = false;
            foreach(Aircraft a in bobo){
                if(a.Distance(loc) == 0){
                    momo = true;
                }
            }
            return momo;
        }
        public bool pirateanddrone(PirateGame game,Pirate p)
        {
                foreach(Drone d in game.GetMyLivingDrones())
                {
                    if(p.Distance(d.Location)<=2)
                       return true;
                }
                return false;
            
        }
        public Pirate findspecificpirate(PirateGame game, int id)
        {
            foreach (Pirate p in game.GetMyLivingPirates())
            {
                if (p.Id == id)
                {
                    return p;
                }
            }
            return null;
        }
        public int givepointsbydistance(PirateGame game, Pirate p, Island motek)
        {
            List<int> dis = new List<int>();
            foreach (Island isl in game.GetAllIslands())
            {
                dis.Add(p.Distance(isl));
            }
            int place = 1;
            if (p.Distance(motek) != 0 && motek.Owner != game.GetMyself())
            {
                foreach (int a in dis)
                {
                    if (a < p.Distance(motek))
                    {
                        place++;
                    }
                }
                return place;
            }
            else
            {
                return 999;
            }
        }
        public int howmuchenemypiratesinthisisland(PirateGame game, Island a)
        {
            int count = 0;
            foreach (Pirate pirate in game.GetEnemyLivingPirates())
            {
                if (a.InControlRange(pirate))
                {
                    count++;
                }
            }
            return count;
        }
        public List<Drone> police(PirateGame game)
        {
            Location a;
            List<Drone> motek = new List<Drone>();
            foreach (Drone d in game.GetMyLivingDrones())
            {
                if (howmuchdronesonthesameplace(game, d) != null) // if there are 5 on the same plcae
                {
                    a = howmuchdronesonthesameplace(game, d); // a is the location of the 5
                    foreach (Drone c in game.GetMyLivingDrones())
                    {
                        if (c.Location == a)
                            motek.Add(c); // adds all the 5 to a list
                    }
                }
            }
            if(motek.Count>=5)
               return motek;
            return null;
        }
        public void attack(PirateGame game, Pirate a, Aircraft enemy)
        {
            if (enemy.CurrentHealth > 0)
                game.Attack(a, enemy);
        }
        public Aircraft doyouhaveatarget(Pirate a, PirateGame game)
        {
            foreach (Aircraft enemy in game.GetEnemyLivingAircrafts())
            {
                if (a.InAttackRange(enemy))
                    return enemy;
            }
            return null;
        }
        public Island fromwhichisland(Drone a, PirateGame game) // from which island do the drone start from?
        {
            foreach (Island b in game.GetAllIslands())
            {
                if (a.Distance(b) <= 3)
                {
                    return b;
                }
            }
            return null;
        }
        public string dontletithappen(PirateGame game)
        {
            foreach (Drone d in game.GetEnemyLivingDrones())
            {
                if (d.Location.Col < game.GetEnemyCities()[0].GetLocation().Col + 4)
                {
                    if (d.Location.Row > game.GetEnemyCities()[0].GetLocation().Row && game.GetMyLivingPirates()[1].Distance(d) <= 8)
                    {
                        game.Debug("PROBLEM DOWN!! DISTANCE IS " + game.GetMyLivingPirates()[0].Distance(d));
                        return "problem down";
                    }
                    else if (d.Location.Row < game.GetEnemyCities()[0].GetLocation().Row && game.GetMyLivingPirates()[0].Distance(d) <= 8)
                    {
                        game.Debug("PROBLEM UP!! DISTANCE IS " + game.GetMyLivingPirates()[0].Distance(d));
                        return "problem up";

                    }
                }
            }
            return "good";
        }
        public Island whoisbetter(Pirate a, Island is1, Island is2)
        {
            if (a.Distance(is1) >= a.Distance(is2))
                return is2;
            return is1;
        }
        public Island whoisbetter2(PirateGame game, Island is1, Island is2)
        {
            if (howmuchenemypiratesinthisisland(game, is1) > howmuchenemypiratesinthisisland(game, is2))
            {
                return is2;
            }
            return is1;
        }
        public bool aremydefendersgood(PirateGame game)
        {
            City city2 = game.GetEnemyCities()[0];
            Pirate p0 = game.GetMyLivingPirates()[0];
            Pirate p1 = game.GetMyLivingPirates()[1];
            return ((p0.Location.Col <= city2.Location.Col + 4) && (p0.Location.Col >= city2.Location.Col) && (p0.Location.Row >= city2.Location.Row - 4) && (p0.Location.Row <= city2.GetLocation().Row - 2) && (p1.Location.Col <= city2.Location.Col + 4) && (p1.Location.Col >= city2.Location.Col) && (p1.Location.Row >= city2.GetLocation().Row + 2) && (p1.Location.Row <= city2.Location.Row + 6));
        }
        // in emergency 4 go up and 3 go down, we need to check their situation
        // pirate 4 -> Location a = new Location(city1.Location.Row - 4, city1.Location.Col);
        //pirate 3 - >Location a = new Location(city1.Location.Row + 6, city1.Location.Col);
        public bool istheupgood(PirateGame game)
        {
            City city2 = game.GetEnemyCities()[0];
            Pirate p4 = findspecificpirate(game, 4);
            if (p4 == null)
                return false;
            Location a = new Location(city2.Location.Row - 4, city2.Location.Col);
            return p4.Distance(a) <= 3;
        }
        public bool isthedowngood(PirateGame game)
        {
            City city2 = game.GetEnemyCities()[0];
            Pirate p3 = findspecificpirate(game, 3);
            if (p3 == null)
                return false;
            Location a = new Location(city2.Location.Row + 6, city2.Location.Col);
            return p3.Distance(a) <= 3;
        }
        public bool ispirate0there(PirateGame game) // pirate0 -> Location loc = new Location(city2.GetLocation().Row - 2, city2.GetLocation().Col + 4);
        {
            City city2 = game.GetEnemyCities()[0];
            Pirate p0 = findspecificpirate(game, 0);
            if (p0 == null)
                return false;
            Location a = new Location(city2.GetLocation().Row - 2, city2.GetLocation().Col + 4);
            return p0.Distance(a) <= 3;
        }
        public Pirate findtheclosestpirate(PirateGame game,Location loc)
        {
            int min=99999999;
            Pirate minpirate=game.GetMyLivingPirates()[0];
            foreach(Pirate p in game.GetMyLivingPirates())
            {
                if(p.Distance(loc)<min)
                   {
                       min=p.Distance(loc);
                       minpirate=p;
                   }
            }
            return minpirate;
        }
        public bool ispirate1there(PirateGame game) // pirate1 -> Location loc = new Location(city2.GetLocation().Row + 3, city2.GetLocation().Col + 4);
        {
            City city2 = game.GetEnemyCities()[0];
            Pirate p1 = findspecificpirate(game, 1);
            if (p1 == null)
                return false;
            Location a = new Location(city2.GetLocation().Row + 2, city2.GetLocation().Col + 4);
            return p1.Distance(a) <= 3;
        }
        public void DoTurn(PirateGame game)
        {
            foreach (Pirate pirate in game.GetMyLivingPirates())
            {
                City city = game.GetMyCities()[0]; // my city
                City city2 = game.GetEnemyCities()[0];//his cisy
                if (pirate.Id == 0)
                {
                    if ((dontletithappen(game) == "problem up" && !(emergency(game))) || (dontletithappen(game) == "problem up" && !istheupgood(game)))
                    {
                        Location loc = new Location(city2.Location.Row - 4, city2.Location.Col);
                        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                        if (doyouhaveatarget(pirate, game) != null) // HAS TARGET
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else if (pirate.GetLocation() != sailOptions[0]) // HASNT TARGET AND NOT IN POS
                        {
                            game.SetSail(pirate, sailOptions[0]);
                        }
                    }
                    else if ((dontletithappen(game) == "problem down" && !(emergency(game))) || (dontletithappen(game) == "problem down" && !isthedowngood(game)))
                    {
                        Location loc = new Location(city2.GetLocation().Row, city2.GetLocation().Col + 4);
                        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                        if (doyouhaveatarget(pirate, game) != null)
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else if (pirate.GetLocation() != sailOptions[0])
                        {
                            game.SetSail(pirate, sailOptions[0]);
                        }
                    }
                    else if (!emergency1(game) && game.GetEnemyLivingDrones().Count == 0)// the enemy got no islands and no drones! so lets attack with all our team!
                    {
                        int max = 0;
                        Island closest = game.GetAllIslands()[0];
                        List<Island> destinations = game.GetAllIslands();
                        foreach (Island a in destinations)
                        {
                            if (rateisland(game, a, pirate) > max)
                            {
                                max = rateisland(game, a, pirate);
                                closest = a;
                            }
                            else if (rateisland(game, a, pirate) == max)
                            {
                                closest = whoisbetter(pirate, closest, a); // if this island got same points, check who is the closest
                                max = rateisland(game, closest, pirate);
                            }
                        }
                        game.Debug("pirate" + pirate.Id + " decided to go to island " + closest.Id + "  which got " + max + " points");
                        List<Location> sailOptions = game.GetSailOptions(pirate, closest);
                        if (doyouhaveatarget(pirate, game) != null)
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else
                        {
                            if (pirate.GetLocation() != sailOptions[0])
                            {
                                game.SetSail(pirate, sailOptions[0]);
                            }
                        }
                    }
                    else
                    {
                        Location loc = new Location(city2.GetLocation().Row - 2, city2.GetLocation().Col + 4);
                        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                        if (doyouhaveatarget(pirate, game) != null) // HAS TARGET
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else if (pirate.GetLocation() != sailOptions[0]) // HASNT TARGET AND NOT IN POS
                        {
                            game.SetSail(pirate, sailOptions[0]);
                        }
                    }
                }
                else if (pirate.Id == 1)
                {
                    if ((dontletithappen(game) == "problem down" && !(emergency(game))) || (dontletithappen(game) == "problem down" && isthedowngood(game)))
                    {
                        Location loc = new Location(city2.Location.Row + 6, city2.Location.Col);
                        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                        if (doyouhaveatarget(pirate, game) != null) // HAS TARGET
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else if (pirate.GetLocation() != sailOptions[0]) // HASNT TARGET AND NOT IN POS
                        {
                            game.SetSail(pirate, sailOptions[0]);
                        }
                    }
                    else if ((dontletithappen(game) == "problem up" && !(emergency(game))) || (dontletithappen(game) == "problem up" && istheupgood(game)))
                    {
                        Location loc = new Location(city2.GetLocation().Row, city2.GetLocation().Col + 4);
                        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                        if (doyouhaveatarget(pirate, game) != null)
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else if (pirate.GetLocation() != sailOptions[0])
                        {
                            game.SetSail(pirate, sailOptions[0]);
                        }
                    }
                    else if (!emergency1(game) && game.GetEnemyLivingDrones().Count == 0)// the enemy has no islands! so lets attack with all our team
                    {
                        int max = 0;
                        Island closest = game.GetAllIslands()[0];
                        List<Island> destinations = game.GetAllIslands();
                        foreach (Island a in destinations)
                        {
                            if (rateisland(game, a, pirate) > max)
                            {
                                max = rateisland(game, a, pirate);
                                closest = a;
                            }
                            else if (rateisland(game, a, pirate) == max)
                            {
                                closest = whoisbetter(pirate, closest, a); // if this island got same points, check who is the closest
                                max = rateisland(game, closest, pirate);
                            }
                        }
                        game.Debug("pirate" + pirate.Id + " decided to go to island " + closest.Id + "  which got " + max + " points");
                        List<Location> sailOptions = game.GetSailOptions(pirate, closest);
                        if (doyouhaveatarget(pirate, game) != null)
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else
                        {
                            if (pirate.GetLocation() != sailOptions[0])
                            {
                                game.SetSail(pirate, sailOptions[0]);
                            }
                        }
                    }
                    else
                    {
                        Location loc = new Location(city2.GetLocation().Row + 2, city2.GetLocation().Col + 4);
                        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                        if (doyouhaveatarget(pirate, game) != null)
                        {
                            attack(game, pirate, doyouhaveatarget(pirate, game));
                        }
                        else if (pirate.GetLocation() != sailOptions[0])
                        {
                            game.SetSail(pirate, sailOptions[0]);
                        }
                    }
                }
                //    else if (pirate.Id == 2) // coming to my city and attacking 
                //     {
                //        Location loc = new Location(city.GetLocation().Row, city.GetLocation().Col - 4);
                //        List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                //         game.SetSail(pirate, sailOptions[0]);
                //         foreach (Aircraft enemy in game.GetEnemyLivingAircrafts())
                //         {
                //             if (pirate.InAttackRange(enemy))
                //           {j
                //                 game.Attack(pirate, enemy);
                //                game.Debug("pirate " + pirate + " attacks " + enemy);
                //            }
                //        }
                //     }
                else if (pirate.Id == 3 || pirate.Id == 4 || pirate.Id == 2) // getting to the closest island, then attacking 
                {
                    //  if(goodtous(game)&&pirate.Id==2)
                    //    {
                    //      game.Debug("It is a good situation, pirate number 2 go go go!");
                    //      City our = game.GetMyCities()[0];
                    //    List<Location> sailOptions = game.GetSailOptions(pirate, our);
                    //       if (doyouhaveatarget(pirate, game) != null)
                    //         {
                    //            attack(game, pirate, doyouhaveatarget(pirate, game));
                    //          }
                    //          else
                    //          {
                    //              if (pirate.GetLocation() != sailOptions[0])
                    //             {
                    //                 game.SetSail(pirate, sailOptions[0]);
                    //              }
                    //         }
                    //        }
                    if (!emergency(game)) 
                    {
                        if ((!ispirate0there(game) || !ispirate1there(game)) && game.GetTurn() > 50 && emergency1(game) && game.GetEnemyLivingDrones().Count != 0)
                        {
                            City city3 = game.GetEnemyCities()[0];
                            Location loc = new Location(city3.GetLocation().Row, city3.GetLocation().Col + 4);
                            List<Location> sailOptions = game.GetSailOptions(pirate, loc);
                            if (doyouhaveatarget(pirate, game) != null)
                            {
                                attack(game, pirate, doyouhaveatarget(pirate, game));
                            }
                            else if (pirate.GetLocation() != sailOptions[0])
                            {
                                game.Debug("Hey, It is pirate " + pirate.Id + ", Im going to help deffending");
                                game.SetSail(pirate, sailOptions[0]);
                            }
                        }
                        else    //normal
                        {
                                    int max = 0;
                                    Island closest = game.GetAllIslands()[0];
                                    List<Island> destinations = game.GetAllIslands();
                                    foreach (Island a in destinations)
                                    {
                                        if (rateisland(game, a, pirate) > max)
                                        {
                                            max = rateisland(game, a, pirate);
                                            closest = a;
                                        }
                                        else if (rateisland(game, a, pirate) == max)
                                        {
                                            closest = whoisbetter(pirate, closest, a); // if this island got same points, check who is the closest
                                            max = rateisland(game, closest, pirate);
                                        }
                                    }
                                    game.Debug("pirate" + pirate.Id + " decided to go to island " + closest.Id + "  which got " + max + " points");
                                    List<Location> sailOptions = game.GetSailOptions(pirate, closest);
                                    if (doyouhaveatarget(pirate, game) != null)
                                    {
                                        if (emergency1(game) && !(aremydefendersgood(game))) // enemy got islands(normal situation) and the defenders are not in pos - so attack drones and pirates
                                        {
                                            attack(game, pirate, doyouhaveatarget(pirate, game)); // drones and pirates
                                        }
                                        else // if the enemy got no islands or the defenders are good - attack only pirates
                                        {
                                            if (doyouhaveatarget(pirate, game) is Pirate)
                                                attack(game, pirate, doyouhaveatarget(pirate, game));
                                            else
                                            {
                                                game.Debug("Hey, it is pirate " + pirate.Id + " talking, I didnt attack " + doyouhaveatarget(pirate, game));
                                                if (pirate.GetLocation() != sailOptions[0])
                                                {
                                                    game.SetSail(pirate, sailOptions[0]);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (pirate.GetLocation() != sailOptions[0])
                                        {
                                            game.SetSail(pirate, sailOptions[0]);
                                        }
                                    }
                      } 
                    }
                    else if (emergency(game)) // emergency
                    {
                        game.Debug("GUYSS!!!!! EMERGENCYY!! DEFENSE OUR CITY!!!!!!!!");
                        if (pirate.Id == 2 && !pirateanddrone(game,pirate) )
                        {
                            int max = 0;
                            Island closest = game.GetAllIslands()[0];
                            List<Island> destinations = game.GetAllIslands();
                            foreach (Island a in destinations)
                            {
                                if (rateisland(game, a, pirate) > max)
                                {
                                    max = rateisland(game, a, pirate);
                                    closest = a;
                                }
                                else if (rateisland(game, a, pirate) == max)
                                {
                                    closest = whoisbetter2(game, a, closest); // if this island got same points, check who is the closest
                                    max = rateisland(game, closest, pirate);
                                }
                            }
                            game.Debug("pirate" + pirate.Id + " decided to go to island " + closest.Id + "  which got " + max + " points");
                            List<Location> sailOptions = game.GetSailOptions(pirate, closest);
                            if (doyouhaveatarget(pirate, game) != null)
                            {
                                if (emergency1(game) && !(aremydefendersgood(game))) // enemy got islands(normal situation) and the defenders are not in pos - so attack drones and pirates
                                {
                                    attack(game, pirate, doyouhaveatarget(pirate, game)); // drones and pirates
                                }
                                else // if the enemy got no islands or the defenders are good - attack only pirates
                                {
                                    if (doyouhaveatarget(pirate, game) is Pirate)
                                        attack(game, pirate, doyouhaveatarget(pirate, game));
                                    else
                                    {
                                        game.Debug("Hey, it is pirate " + pirate.Id + " talking, I didnt attack " + doyouhaveatarget(pirate, game));
                                        if (pirate.GetLocation() != sailOptions[0])
                                        {
                                            game.SetSail(pirate, sailOptions[0]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (pirate.GetLocation() != sailOptions[0])
                                {
                                    game.SetSail(pirate, sailOptions[0]);
                                }
                            }

                        }
                        else if (pirate.Id == 4 &&  !pirateanddrone(game,pirate) )
                        {
                            City city1 = game.GetEnemyCities()[0];
                            Location a = new Location(city1.Location.Row - 4, city1.Location.Col);
                            List<Location> sailOptions = game.GetSailOptions(pirate, a);
                            if (doyouhaveatarget(pirate, game) != null)
                            {
                                attack(game, pirate, doyouhaveatarget(pirate, game));
                            }
                            else // doesnt have a target
                            {
                                if (!ispirate0there(game)) // pirate0 -> Location loc = new Location(city2.GetLocation().Row - 2, city2.GetLocation().Col + 4);
                                {
                                    List<Location> sailOptions2 = game.GetSailOptions(pirate, new Location(city2.GetLocation().Row - 2, city2.GetLocation().Col + 4));
                                    if (pirate.Location != sailOptions2[0])
                                        game.SetSail(pirate, sailOptions2[0]);
                                }
                                else if (pirate.GetLocation() != sailOptions[0])
                                {
                                    game.SetSail(pirate, sailOptions[0]);
                                }
                            }
                        }
                        else if (pirate.Id == 3 && !pirateanddrone(game,pirate) )
                        {
                            City city1 = game.GetEnemyCities()[0];
                            Location a = new Location(city1.Location.Row + 6, city1.Location.Col);
                            List<Location> sailOptions = game.GetSailOptions(pirate, a);
                            if (doyouhaveatarget(pirate, game) != null)
                            {
                                attack(game, pirate, doyouhaveatarget(pirate, game));
                            }
                            else // doesnt have a target
                            {
                                if (!ispirate1there(game)) // pirate1 -> Location loc = new Location(city2.GetLocation().Row + 3, city2.GetLocation().Col + 4);
                                {
                                    List<Location> sailOptions2 = game.GetSailOptions(pirate, new Location(city2.GetLocation().Row + 2, city2.GetLocation().Col + 4));
                                    if (pirate.Location != sailOptions2[0])
                                        game.SetSail(pirate, sailOptions2[0]);
                                }
                                else if (pirate.GetLocation() != sailOptions[0])
                                {
                                    game.SetSail(pirate, sailOptions[0]);
                                }
                            }
                        }
                    }
                }
            } // giant foreach of pirates
            // handleDrones(game.GetMyLivingDrones(), game);
            // normaldrones(game);
            normaldrones(game,game.GetMyLivingDrones());

        }//doturn
    }
}