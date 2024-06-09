import axios from 'axios'
import { BaseClient } from './BaseClient'
import type { CollectionModel } from '@/models/CollectionModel'
import type { SnippetModel } from '@/models/SnippetModel'

export class CollectionClient extends BaseClient {
  public async GetCollections(): Promise<Array<CollectionModel>> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/collection/', {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }

  public async GetSnippetsFromCollection(collectionID: string): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().get(
        this.baseUrl + '/collection/GetSnippetsFromCollection/' + collectionID,
        {}
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetSnippetsWithoutCollection(): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().get(
        this.baseUrl + '/collection/GetSnippetsWithoutCollection/',
        {}
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async AddNewCollection(collectionName: string): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().post(this.baseUrl + '/collection/', {
        CollectionID: '',
        CollectionName: collectionName
      })
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
