import { computed, inject, Injectable, signal } from "@angular/core";
import { StorageService } from "./storage.service";
import { StorageKeys } from "../constants/storage.constants";
import { UserPayload } from "../interfaces/user-payload.interface";

@Injectable({ providedIn: 'root' })
export class AppStateService {
  private storage = inject(StorageService);
  private themeSignal = signal<'light' | 'dark'>(
    (this.storage.getRaw(StorageKeys.THEME_MODE) as 'light' | 'dark') || 'light'
  );
  private userSignal = signal<UserPayload | null>(
    this.storage.getObject<UserPayload>(StorageKeys.AUTH_USER)
  );
  public isDark = computed(() => this.themeSignal() === 'dark');
  public userData = computed(() => this.userSignal());
  updateTheme(mode: 'light' | 'dark') {
    this.themeSignal.set(mode);
    this.storage.setRaw(StorageKeys.THEME_MODE, mode);
  }
}
