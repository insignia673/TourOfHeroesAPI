import { Component, OnInit } from '@angular/core';
import { debounceTime, distinctUntilChanged, Observable, Subject, switchMap } from 'rxjs';
import { Hero } from '../interfaces/hero';
import { HeroService } from '../services/hero.service';

@Component({
  selector: 'app-hero-search',
  templateUrl: './hero-search.component.html',
  styleUrls: ['./hero-search.component.css']
})
export class HeroSearchComponent implements OnInit {
  heroes$!: Observable<Hero[]>;
  private searchTerms = new Subject<string>();

  constructor(private heroService: HeroService) { }

  ngOnInit(): void {
    this.heroes$ = this.searchTerms.pipe(
      // waits until the flow of new string events pauses for 300 
      // milliseconds before passing along the latest string. 
      // You'll never make requests more frequently than 300ms.
      debounceTime(300),

      // ignore new term if same as previous term, ensures that a request is sent only if the filter text changed.
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this.heroService.searchHeroes(term)),
    );
  }

  // Push a search term into the observable stream.
  search(value: string): void{
    this.searchTerms.next(value);
  }
}
