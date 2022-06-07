import { Component, OnInit } from '@angular/core';
import { Hero } from 'src/app/interfaces/hero';
import { HeroService } from '../services/hero.service';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-heroes',
  templateUrl: './heroes.component.html',
  styleUrls: ['./heroes.component.css']
})

export class HeroesComponent implements OnInit {

  heroes: Hero[] = [];
  //selectedHero?: Hero;

  constructor(private heroService: HeroService, private messageService: MessageService) { }

  ngOnInit(): void {
    this.getHeroes();
  }

  getHeroes(): void {
    this.heroService.getHeroes()
        .subscribe(heroes => this.heroes = heroes);
  }

  add(name: string): void {
    name.trim();
    if(!name) {return;}
    
    //cant add 'hero => this.heroes.push(hero)' to subscribe because hero is already added to array
    //idk why the heroes arr auto updates when this is fired.
    this.heroService.addHero({name} as Hero).subscribe(hero => {
      if(hero){
        this.heroes.push(hero);
      }
    });
  }

  delete(hero: Hero): void{
    this.heroService.deleteHero(hero).subscribe(result => {
      if(result > 0){
        this.heroes = this.heroes.filter(h => h !== hero);
      }
    });
  }

  // onSelect(hero: Hero): void {
  //   if(this.selectedHero != hero){
  //     this.selectedHero = hero;
  //     this.messageService.add(`HeroesComponent: Selected hero ${hero.name} id=${hero.id}`);
  //   }else{
  //     this.selectedHero = undefined;
  //     this.messageService.add(`HeroesComponent: Deselected hero ${hero.name} id=${hero.id}`);
  //   }
  // }
}
