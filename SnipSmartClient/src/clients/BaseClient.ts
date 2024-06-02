import { inject } from 'vue'
import type { VueCookies } from 'vue-cookies'

export class BaseClient {
  jwt: string
  constructor() {
    this.jwt = ''
  }
  baseUrl: string = 'http://localho.st:5132'
}
