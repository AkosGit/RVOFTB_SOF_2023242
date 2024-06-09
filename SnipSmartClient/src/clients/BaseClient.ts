import axios from 'axios'
import { inject } from 'vue'
import type { VueCookies } from 'vue-cookies'

export class BaseClient {
  jwt = ''
  baseUrl: string = 'http://localho.st:5132'
  GetAxios() {
    return axios.create({
      baseURL: this.baseUrl,
      headers: {
        Authorization: `Bearer ${this.jwt}`
      }
    })
  }
}
