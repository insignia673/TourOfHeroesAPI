import { Inject, Injectable } from '@angular/core';
import { Hero } from '../interfaces/hero';
import { HEROES } from '../mock-heroes';
import { catchError, Observable, of, tap } from 'rxjs';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HeroService {

  constructor(private messageService: MessageService, private http: HttpClient, @Inject('API_BASE_URL') private baseUrl : string) { }

  getHeroes(): Observable<Hero[]>{
    // const heroes = of(HEROES).pipe(
    //   catchError(this.handleError<Hero[]>('getHeroes', []))
    // );
    // this.messageService.add('HeroService: fetched heroes');
    // return heroes;
    return this.http.get<Hero[]>(this.baseUrl).pipe(
      tap(_ => console.log('fetched heroes')),
      tap(h => console.log(h)),
      catchError(this.handleError<Hero[]>('getHeroes', []))
    );
  }

  getHero(id: number): Observable<Hero> {
    console.log('HeroService: Getting Hero');
    // const hero = HEROES.find(h => h.id === id)!;
    this.messageService.add(`HeroService: fetched hero id=${id}`);
    // return of(hero).pipe(
    //   tap(result => console.log(result)),
    //   catchError(this.handleError<Hero>('getHero', undefined))
    // );

    return this.http.get<Hero>(this.baseUrl + id).pipe(
      tap(result => console.log(result)),
      catchError(this.handleError<Hero>('getHero', undefined))
    );
  }

  addHero(hero: Hero): Observable<Hero>{
    // hero.id = HEROES[HEROES.length-1].id + 1;
    // HEROES.push(hero);
    // return of(hero).pipe(
    //   tap(_ => this.messageService.add(`HeroService: added hero ${hero.name}`)),
    //   catchError(this.handleError<Hero>('addHero', undefined))
    // );
    return this.http.post<Hero>(this.baseUrl, hero).pipe(
      tap(returned => {if(returned) this.messageService.add(`HeroService: added hero ${hero.name}`)}),
      catchError(this.handleError<Hero>('addHero', undefined))
    );
  }

  updateHero(hero: Hero): Observable<Hero>{
    return this.http.put<Hero>(this.baseUrl, hero).pipe(
      tap(returned => {if(returned) this.messageService.add(`HeroService: updated hero ${hero.name}`)}),
      catchError(this.handleError<Hero>('updateHero', undefined))
    );
  }

  deleteHero(hero: Hero): Observable<number>{
    // HEROES.splice(HEROES.indexOf(hero), 1);
    // return of(hero).pipe(
    //   tap(_ => this.messageService.add(`HeroService: deleted hero ${hero.name}`)),
    //   catchError(this.handleError<Hero>('addHero', undefined))
    // );
    return this.http.delete<number>(this.baseUrl + hero.id).pipe(
      tap(value => {if(value > 0) this.messageService.add(`HeroService: deleted hero ${hero.name}`)}),
      catchError(this.handleError<number>('deleteHero', 0))
    );
  }

  searchHeroes(name: string): Observable<Hero[]>{
    if (!name.trim()) {
      // if not search term, return empty hero array.
      return of([]);
    }
    // return of(HEROES.filter(h => h.name.toLowerCase().match(name.toLocaleLowerCase()))).pipe(
    //   tap(x => x.length ?
    //      console.log(`found heroes matching "${name}"`) :
    //      console.log(`no heroes matching "${name}"`)),
    //   catchError(this.handleError<Hero[]>('searchHeroes', []))
    // );
    return this.http.get<Hero[]>(this.baseUrl + 'name/' + name).pipe(
      tap(x => x.length ? console.log(`found heroes matching "${name}"`) : console.log(`no heroes matching "${name}"`)),
      catchError(this.handleError<Hero[]>('searchHeroes', []))
    );
  }

  private handleError<T>(operation = 'operation', result?: T){
    return(error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    }
  }
}
